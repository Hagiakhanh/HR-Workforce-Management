# Interview 1:1 — ASP.NET Core Backend & AI@HR

> Tài liệu được tổng hợp từ code và lịch sử Git của repository tại ngày 29/10/2026. Phần “đóng góp cá nhân” nhận diện tác giả là **Ha Gia Khanh / Hagiakhanh**. Nếu đây không phải Git identity của bạn, hãy sửa phần đó trước khi dùng.

## 1. Cách dùng tài liệu này

Đừng học thuộc từng câu. Mỗi chủ đề nên trả lời theo khung:

1. **Khái niệm** — nó là gì.
2. **Lý do** — giải quyết vấn đề gì, vì sao chọn nó.
3. **Trong dự án** — chỉ đúng luồng/file có sử dụng.
4. **Trade-off** — lợi ích, giới hạn và cách cải thiện.

Ba mức độ cần chuẩn bị:

- 60 giây: giới thiệu bản thân và phần mình làm.
- 3–5 phút: trình bày một feature end-to-end.
- 10–15 phút: đào sâu kiến trúc, lỗi, concurrency, test và trade-off.

## 2. Bài giới thiệu mẫu

### Bản 60 giây

> Trong dự án AI@HR, em tập trung chủ yếu vào .NET backend, đặc biệt là module Jobs và Weights. Em xây dựng các API quản lý job, luồng submit JD bất đồng bộ qua RabbitMQ, consumer nhận kết quả từ AI worker, lưu dữ liệu bằng EF Core/PostgreSQL và đẩy trạng thái realtime cho frontend qua SignalR. Em cũng làm CRUD và validation cho weight profile từ Markdown, xử lý duplicate ở cả application và database để chống race condition. Sau đó em refactor theo repository và rich domain model, đồng thời bổ sung unit/integration test cho Jobs, Weights và Identity. Qua dự án em hiểu rõ hơn về ASP.NET Core request pipeline, DI, CQRS/Mediator, EF Core tracking, async messaging, idempotency và testing. Nếu được lên chính thức, em muốn tiếp tục nâng chất lượng production như authorization chi tiết hơn, observability, concurrency control và integration test end-to-end.

### Bản 3 phút

> AI@HR hỗ trợ recruiter sàng lọc CV. Frontend React chỉ gọi .NET Web API. .NET sở hữu PostgreSQL và REST API; Java AI worker sở hữu filesystem và gọi LLM. Hai backend giao tiếp bất đồng bộ qua RabbitMQ vì xử lý AI mất nhiều giây và không phù hợp giữ HTTP request chờ.
>
> Phần em làm nhiều nhất là Jobs và Weights. Với Jobs, em xây các endpoint list/detail/update/delete và hai cách submit JD. Khi submit, API validate input, tạo thư mục theo dõi rồi publish command sang RabbitMQ và trả `202 Accepted`. Java worker xử lý JD, sau đó publish completed hoặc failed event. MassTransit consumer bên .NET validate envelope, chống xử lý trùng bằng `PathWatcher`, persist `Job` cùng criteria bằng EF Core, rồi SignalR báo cho frontend. Nếu message không hợp lệ hoặc worker thất bại, hệ thống dọn resource tạm, gửi trạng thái failed và dùng retry/DLQ theo loại lỗi.
>
> Với Weights, em làm CRUD, phân trang/tìm kiếm, parse và validate Markdown bằng Markdig. Validation kiểm tra title, Tier 1 Must-Have, cấu trúc bảng, định dạng decimal/percentage và tổng weight bằng 1. Em xử lý duplicate theo hai lớp: check sớm để trả lỗi thân thiện, và unique index cộng catch PostgreSQL unique violation để an toàn khi hai request chạy đồng thời.
>
> Em cũng refactor entity theo rich domain model, repository abstraction, dùng `AsNoTracking` cho query chỉ đọc, projection để chỉ lấy field cần thiết, FluentValidation chạy qua Mediator pipeline, và `ProblemDetails` cho lỗi HTTP nhất quán. Em viết nhiều unit test cho handler/validator, repository integration test với PostgreSQL thật qua Testcontainers, và test Identity. Điều em học được nhiều nhất là correctness không chỉ nằm ở happy path mà còn ở duplicate message, race condition, retry, cleanup và cách phân loại lỗi recoverable/non-recoverable.

## 3. Bức tranh hệ thống

```text
Recruiter
   |
   v
React/Vite frontend
   | REST + SignalR
   v
.NET Web API (modular monolith)
   |-- Identity module
   |-- Jobs module
   |-- Screening module
   |-- EF Core --> PostgreSQL
   |-- MassTransit --> RabbitMQ
   `-- SignalR --> frontend
                      ^
                      |
RabbitMQ <------> Java AI worker ----> Azure AI/LLM + filesystem
```

### Trách nhiệm từng thành phần

| Thành phần | Trách nhiệm |
|---|---|
| React frontend | UI recruiter, gọi REST, lắng nghe sự kiện SignalR |
| .NET Web API | HTTP surface, authentication/authorization, business workflow, toàn bộ truy cập PostgreSQL |
| Java AI worker | đọc/chuyển đổi file, gọi LLM, đánh giá JD/CV, thao tác filesystem |
| RabbitMQ | tách rời hai backend và đưa tác vụ AI ra khỏi HTTP request path |
| PostgreSQL | lưu identity, jobs, criteria, weights, screening và outbox |
| Shared contracts | AsyncAPI + JSON Schema làm nguồn chung giữa Java và .NET |

### Kiểu kiến trúc .NET

Đây là **modular monolith**: deploy chung một Web API nhưng code chia theo business module `Identity`, `Jobs`, `Screening`. Mỗi module tiếp tục chia thành:

- **Domain**: entity, enum, domain rule; không phụ thuộc hạ tầng.
- **Application**: command/query, handler, validator, interface và use case.
- **Infrastructure**: EF Core, repository, RabbitMQ/MassTransit, HTTP client, SignalR adapter.
- **WebApi**: endpoint, middleware và composition root.

Lợi ích: ranh giới module rõ, dễ test, ít chi phí vận hành hơn microservices. Giới hạn: vẫn dùng chung process/deployment; nếu không giữ dependency boundary thì module có thể coupling với nhau.

## 4. Những phần bạn trực tiếp làm

Lịch sử Git cho thấy 86 commit dưới hai identity của bạn. Các nhóm đóng góp chính:

### 4.1. Quản lý Jobs

- Xây endpoint quản lý job: list, detail, job description, attention, update criteria, update status, delete.
- Dùng CQRS với command/query và Mediator.
- Repository cho tìm kiếm, filter status, pagination và CRUD.
- Gọi `ScreeningClient` để lấy candidate count hoặc xóa document liên quan khi xóa job.
- Chuyển entity `Job` từ anemic model sang rich domain model: private setter, factory `Create`, `ChangeStatus`, `UpdateCriteria`, `AddCriteria`, `CanBeDeleted`.
- Dùng enum và constants thay magic string.
- Bổ sung `GetJobScreeningContext` và test cho toàn bộ command/query quan trọng.

Code nên đọc lại:

- `src/AI.HR.WebApi/Endpoints/Jobs/`
- `src/Modules/Jobs/AI.HR.Jobs.Application/Jobs/`
- `src/Modules/Jobs/AI.HR.Jobs.Domain/Entities/Job.cs`
- `src/Modules/Jobs/AI.HR.Jobs.Infrastructure/Persistence/Repositories/JobRepository.cs`

### 4.2. Submit và xử lý JD bất đồng bộ

Luồng thành công:

```text
POST /api/jobs hoặc POST /api/jobs/from-markdown
  -> FastEndpoint tạo command
  -> FluentValidation qua Mediator pipeline
  -> kiểm tra PathWatcher ở DB và storage
  -> tạo directory
  -> publish ParseJdCommand qua RabbitMQ
  -> trả 202 Accepted
  -> Java worker gọi LLM
  -> publish JdProcessingCompleted
  -> ParseJdConsumer nhận message
  -> JdProcessingResultService chống duplicate và lưu Job + Criteria
  -> SignalR phát jdProcessed
  -> frontend cập nhật UI
```

Luồng lỗi:

- Publish thất bại: xóa directory vừa tạo như một hành động bù trừ.
- Worker trả failed event: dọn directory nếu chưa có job, log và báo failed qua SignalR.
- Message completed sai contract: không retry lỗi permanent, cleanup an toàn, notify failed rồi đưa message sang error/DLQ.
- Lỗi transient: retry theo interval 1 giây và 2 giây.
- Message trùng: tìm theo `PathWatcher`; nếu đã có thì không tạo job mới nhưng vẫn notify client.
- Hai consumer chạy đồng thời: unique index ở DB là hàng rào cuối; catch `DbUpdateException`, đọc lại record đã được request kia tạo.

Điểm đáng nói trong interview:

- `202 Accepted` đúng hơn `201 Created` vì tại thời điểm trả response, job chưa được tạo xong.
- Broker tạo **eventual consistency**: UI không có kết quả ngay, nên dùng correlation ID và SignalR.
- RabbitMQ thường cho semantics **at-least-once**, vì vậy consumer phải idempotent.
- Check-before-insert không đủ chống race; unique constraint mới là source of truth.
- Không retry lỗi contract/format vì retry không thể biến payload sai thành đúng.

Code nên đọc lại:

- `Jobs/Commands/JobSubmissionExecutor.cs`
- `Jobs/Services/JdProcessingResultService.cs`
- `Infrastructure/Messaging/ParseJdConsumer.cs`
- `Infrastructure/Messaging/ParseJdFailedConsumer.cs`
- `Infrastructure/Configurations/JdEndpointConfiguration.cs`
- `Shared/AI.HR.Shared.Infrastructure/Realtime/SignalRNotifier.cs`

### 4.3. Weight profiles

- CRUD 5 endpoint: list, get by id, import, update, delete.
- Search không phân biệt hoa thường bằng PostgreSQL `ILIKE`.
- Escape `%`, `_`, `\` để ký tự người dùng không vô tình trở thành wildcard.
- Dùng `AsNoTracking` cho read-only query và projection sang response DTO.
- Parse Markdown bằng Markdig thay vì regex toàn bộ document.
- Validate title, Tier 1 Must-Have, header/cell, percentage hoặc decimal, giá trị dương và tổng đúng 1.00/100%.
- Chuẩn hóa BOM và whitespace trong domain entity.
- Unique index trên `Name`; map unique violation thành HTTP 409.
- Repository integration test với PostgreSQL/Testcontainers để kiểm tra behavior thật của `ILIKE`, unique index và EF mapping.

Trade-off nên nói được:

- Application pre-check cho UX tốt, database constraint bảo đảm correctness.
- So sánh decimal chính xác phù hợp với weight được nhập hữu hạn, nhưng có thể cân nhắc tolerance nếu nhận kết quả tính toán floating-point từ nguồn ngoài.
- Markdig hiểu cấu trúc Markdown tốt hơn regex, đổi lại validator phức tạp hơn và cần test nhiều biến thể.

### 4.4. Identity và testing

- Refactor command/query Identity dùng `IUserRepository`, tách EF Core khỏi Application layer.
- Repository normalize email và dùng `AsNoTracking` mặc định.
- Viết test cho register validator/handler, get user, credential validation, notification handler và repository.
- Password không tự hash; dùng `Microsoft.AspNetCore.Identity.PasswordHasher<TUser>`.
- Authentication dùng OpenIddict, access/refresh token và role claim.

### 4.5. Realtime và frontend integration

- Ban đầu notifier nằm trong Jobs, sau đó refactor `AppHub` và `SignalRNotifier` sang Shared để module khác tái sử dụng.
- Application phụ thuộc `IJobEventNotifier`/`IRealtimeNotifier`, không phụ thuộc trực tiếp SignalR.
- Frontend lắng nghe trạng thái theo correlation ID/path watcher để đóng vòng xử lý bất đồng bộ.

### 4.6. Chất lượng code

- Unit test cho handlers, validators và consumers bằng xUnit/Moq/FluentAssertions.
- Integration test repository với PostgreSQL Testcontainers thay vì chỉ EF InMemory.
- Refactor magic string, access modifier, duplication và model mapping sau review.
- Sửa dependency security (`dompurify`) và các error path/retry/DLQ.

Kiểm tra trực tiếp ngày 29/10/2026: **202/202 Jobs Application tests** và **25/25 Identity Application tests** pass. Đây là số test hiện có của hai project, không nên nói tất cả 227 test đều do riêng bạn viết.

Không nên nói “em làm toàn bộ hệ thống”. Outbox framework, Screening core, Java worker, topology generator, deployment và phần lớn frontend có đóng góp của thành viên khác. Có thể nói: **em hiểu luồng tổng thể, còn ownership chính của em là Jobs, Weights, JD result integration và test Identity**.

## 5. ASP.NET Core nền tảng

### 5.1. Request pipeline

ASP.NET Core xử lý request qua chuỗi middleware. Mỗi middleware có thể làm việc trước/sau middleware tiếp theo hoặc short-circuit.

Pipeline hiện tại:

```text
Exception handler
 -> CORS
 -> Authentication
 -> Authorization
 -> SignalR hubs / FastEndpoints
```

Điểm cần nhớ:

- Thứ tự quan trọng: authenticate phải chạy trước authorize.
- Exception handler đặt sớm để bắt lỗi từ phần sau.
- CORS là chính sách của browser; không thay authentication/authorization.
- Endpoint là terminal point thực thi use case.

**Câu hỏi:** Middleware khác action filter thế nào?

> Middleware hoạt động ở HTTP pipeline và có thể áp dụng cho mọi request, không phụ thuộc MVC. Filter chạy quanh lifecycle của MVC/action hoặc framework endpoint tương ứng và có context cấp action. Dự án dùng global exception middleware/handler vì muốn format lỗi thống nhất cho toàn API.

### 5.2. Dependency Injection và lifetime

- `Transient`: tạo instance mỗi lần resolve; phù hợp service nhẹ, stateless.
- `Scoped`: một instance cho mỗi HTTP request/scope; `DbContext` và repository thường là scoped.
- `Singleton`: một instance suốt application; phải thread-safe và không được giữ scoped dependency.

Trong dự án:

- `JobsDbContext`, repositories: scoped.
- SignalR notifier adapter được đăng ký singleton và dùng abstraction phù hợp; `IHubContext` an toàn để inject lâu dài.
- Handler được Mediator resolve từ container.

**Câu hỏi xoáy:** Vì sao không inject `DbContext` vào singleton?

> `DbContext` không thread-safe, có change tracker theo unit of work và được thiết kế scoped. Singleton giữ nó sẽ dùng chung state giữa request, gây concurrency bug và connection/resource lifetime sai. Nếu background singleton cần DB, tạo scope bằng `IServiceScopeFactory` cho mỗi vòng xử lý.

### 5.3. Configuration và Options

ASP.NET Core ghép configuration từ `appsettings.json`, environment-specific file, environment variables, command line và secret provider. Không commit secrets. Dự án bind `DatabaseSettings`, connection string, RabbitMQ/CORS và dùng Aspire để inject resource connection.

Nên biết:

- `IOptions<T>`: snapshot cấu hình khởi động.
- `IOptionsSnapshot<T>`: scoped, đọc lại theo request.
- `IOptionsMonitor<T>`: singleton-friendly, hỗ trợ change notification.

### 5.4. Async/await và cancellation

- Async không tự tạo thread; nó giải phóng request thread trong lúc chờ I/O.
- Dùng async cho DB, broker, file/network I/O để tăng throughput.
- Truyền `CancellationToken` từ endpoint xuống handler, repository và external calls.
- Không dùng `.Result`/`.Wait()` vì block thread và có thể gây starvation/deadlock ở môi trường khác.
- Cleanup quan trọng đôi khi dùng `CancellationToken.None` vì client ngắt kết nối không nên bỏ resource rác; cần giới hạn timeout riêng để tránh treo.

### 5.5. Error handling và Problem Details

`GlobalExceptionHandler` map domain/application exception sang HTTP status và RFC-style `ProblemDetails`:

- validation/bad request → 400
- not found → 404
- conflict/duplicate → 409
- unsupported file → 415
- unknown → 500, không lộ chi tiết nội bộ

`traceId` giúp nối lỗi client với log server. Chỉ log stack trace đầy đủ cho lỗi 500 để tránh noise và lộ dữ liệu.

### 5.6. Logging

Dùng structured logging:

```csharp
logger.LogWarning("Job {JobId} already exists at {PathWatcher}", id, path);
```

Không nối chuỗi vì mất structured fields. Dự án còn dùng source-generated `[LoggerMessage]`, giảm allocation và kiểm tra template tốt hơn.

Không log CV/JD raw content, password, token hoặc PII.

## 6. Web API và REST

### 6.1. HTTP verbs và status code

| Tình huống | Status phù hợp |
|---|---|
| Tạo resource đồng bộ | 201 Created, tốt nhất có `Location` |
| Nhận tác vụ xử lý nền | 202 Accepted |
| Đọc thành công | 200 OK |
| Update không trả body | 204 No Content |
| Input sai | 400 Bad Request |
| Chưa đăng nhập/token sai | 401 Unauthorized |
| Đã đăng nhập nhưng thiếu quyền | 403 Forbidden |
| Không tồn tại | 404 Not Found |
| Trùng/vi phạm state | 409 Conflict |
| Sai loại file/content type | 415 Unsupported Media Type |

`PUT` thường replace toàn resource và idempotent; `PATCH` cập nhật một phần; `POST` thường không idempotent. API hiện dùng command-style update, khi giải thích cần bám behavior thực tế thay vì khẳng định tuyệt đối theo tên method.

### 6.2. DTO thay vì expose entity

Entity biểu diễn persistence/domain và có invariant; DTO là contract HTTP. Tách chúng giúp:

- tránh over-posting;
- không lộ field nội bộ;
- version API độc lập với schema DB;
- tối ưu query bằng projection;
- validation theo use case.

### 6.3. Pagination, filter và search

Repository thực hiện filter trước, `CountAsync`, sau đó `OrderBy`, `Skip`, `Take`, `Select`.

Lưu ý:

- Luôn có order ổn định; production nên thêm `ThenBy(Id)` nếu `CreatedAt` có thể trùng.
- Offset pagination đơn giản nhưng chậm khi offset rất lớn và có thể lệch khi dữ liệu thay đổi; cursor/keyset pagination tốt hơn cho feed lớn.
- Giới hạn `pageSize` trong validator để chống query quá nặng.

### 6.4. File upload

- Kiểm tra extension **và** content/magic bytes nếu rủi ro cao; không tin `ContentType` từ client.
- Giới hạn kích thước request và stream thay vì load file lớn toàn bộ vào RAM.
- Sanitize path/file name để chống path traversal.
- Không dùng tên file do user cung cấp làm storage path trực tiếp.
- Scan malware nếu nhận tài liệu thật.

### 6.5. API documentation

FastEndpoints sinh OpenAPI; Scalar/Swagger chỉ expose UI ở Development. Contract mô tả response code và problem response. Documentation không thay contract test, nhưng giúp frontend và reviewer dùng API đúng.

## 7. EF Core nền tảng

### 7.1. `DbContext` là gì?

`DbContext` kết hợp Unit of Work và change tracker. Nó theo dõi entity được query, phát hiện thay đổi và chuyển thành SQL khi `SaveChangesAsync`.

Trong dự án có DbContext riêng theo module/schema:

- Identity → schema identity
- Jobs → schema jobs
- Screening → schema screening

Mỗi module có migration history table riêng, giúp migration không trộn lẫn.

### 7.2. Tracking và `AsNoTracking`

- Tracking cần khi load entity rồi sửa và save.
- `AsNoTracking` giảm memory/CPU cho query chỉ đọc.
- `GetTrackedAsync` dùng cho update/delete.
- `GetByIdAsync` read-only dùng `AsNoTracking`.

Nếu gọi `Update()` cho detached entity, EF đánh dấu nhiều property Modified; cần cẩn thận lost update/overwriting. Tốt hơn load tracked entity và gọi domain method khi update một phần.

### 7.3. Fluent configuration

`IEntityTypeConfiguration<T>` tách mapping khỏi entity:

- table/schema;
- primary key;
- required/max length;
- unique index;
- one-to-many Job–Criteria;
- cascade delete.

`ApplyConfigurationsFromAssembly` tự discover mapping, giảm code đăng ký thủ công.

### 7.4. Migration

Migration là phiên bản hóa schema. Quy trình an toàn:

1. đổi model/configuration;
2. tạo migration;
3. review generated SQL và data-loss risk;
4. test trên DB gần production;
5. deploy theo chiến lược backward-compatible;
6. không sửa migration đã chạy ở production; tạo migration mới.

### 7.5. Query translation

LINQ trước materialization thường được EF dịch thành SQL. `Select` projection giúp DB chỉ trả column cần thiết. Tránh gọi method C# không dịch được trong query. Dùng `ToQueryString()` hoặc logging để kiểm tra SQL.

N+1 xảy ra khi query parent rồi thực hiện thêm query cho từng child. Giải pháp: projection, `Include`, batch query hoặc explicit query tùy nhu cầu. Không `Include` mọi thứ mặc định vì có thể tạo Cartesian explosion.

### 7.6. Index và constraint

- Index tăng tốc read/filter/order nhưng tốn storage và làm write chậm hơn.
- Unique constraint không chỉ tối ưu query mà còn bảo vệ invariant ở DB.
- Dự án dùng unique `PathWatcher` và `Weight.Name`.
- Nên index theo query thực tế; `ILIKE '%term%'` không tận dụng B-tree thông thường tốt, dữ liệu lớn có thể dùng PostgreSQL trigram index/full-text search.

### 7.7. Transaction và concurrency

Một lần `SaveChanges` mặc định chạy transaction cho các thay đổi trong lần save đó. Nhiều external system không thể nằm chung transaction DB:

- DB + RabbitMQ: dùng transactional outbox.
- DB + filesystem: dùng compensation hoặc thiết kế lại ownership/workflow.

Optimistic concurrency có thể dùng row version/concurrency token. Khi hai user cùng sửa weight, hiện tại request đến sau có thể ghi đè request trước; cải thiện bằng `xmin`/version, ETag + `If-Match`, rồi trả 409/412 khi stale.

### 7.8. Repository pattern: trả lời cân bằng

Ưu điểm trong dự án:

- Application không phụ thuộc EF Core trực tiếp;
- gom query dùng lại và test handler dễ;
- thể hiện boundary của module.

Nhược điểm:

- `DbContext`/`DbSet` vốn đã gần Repository + Unit of Work;
- repository generic dễ che mất sức mạnh LINQ và sinh method vụn;
- nếu mỗi method tự `SaveChanges`, khó gom nhiều mutation vào một transaction.

Câu trả lời tốt:

> Em không cho rằng mọi dự án bắt buộc có repository. Ở dự án này, repository có giá trị vì giữ Application độc lập hạ tầng và tập trung query theo use case. Em ưu tiên repository cụ thể theo aggregate, không dùng generic repository chỉ để bọc lại từng method của EF. Với workflow cần atomic nhiều thay đổi, em sẽ tách `SaveChanges` ra Unit of Work thay vì để mỗi repository save ngay.

## 8. CQRS, Mediator và validation

CQRS ở mức code tách:

- Command: thay đổi state.
- Query: chỉ đọc, có thể projection tối ưu.

Mediator tách endpoint khỏi handler: endpoint chỉ chuyển HTTP request thành command/query; handler chứa use case. Pipeline behavior xử lý cross-cutting concern như validation.

Lợi ích: file nhỏ theo feature, test handler độc lập, consistent validation. Chi phí: nhiều type/file, tracing flow khó hơn và không nên biến handler thành lớp chuyển tiếp vô nghĩa.

Validation có ba tầng:

1. Transport/input: required, length, file type, page size.
2. Application/business use case: duplicate, state transition, existence.
3. Domain/database invariant: entity method, unique/FK/check constraint.

Đừng chỉ validate ở frontend; client có thể bypass UI.

## 9. Authentication và authorization

### Authentication vs authorization

- Authentication: bạn là ai?
- Authorization: bạn được làm gì?

Dự án dùng OpenIddict:

- `/connect/token` nhận password grant hoặc refresh token;
- xác thực credential qua query;
- tạo `ClaimsIdentity` với subject, email, name, role;
- role claim được đưa vào access/identity token;
- endpoint giới hạn `Recruiter` bằng `Roles(...)`.

Access token ngắn hạn để gọi API; refresh token sống lâu hơn để xin access token mới. Refresh flow phải kiểm tra user còn active.

Điểm bảo mật cần nói:

- HTTPS bắt buộc.
- Không log token/password.
- Hash password bằng thuật toán password hashing có salt/cost, không dùng SHA trực tiếp.
- Token phải kiểm issuer, audience, signature, lifetime.
- Role phù hợp rule thô; policy/permission phù hợp rule chi tiết.
- Password grant phù hợp first-party/legacy nhưng OAuth hiện đại thường ưu tiên Authorization Code + PKCE cho interactive client.

## 10. RabbitMQ, MassTransit và distributed systems

### Vì sao dùng message broker?

LLM chậm và có thể không ổn định. Broker giúp:

- API trả nhanh;
- hấp thụ burst;
- worker scale độc lập;
- lỗi worker làm giảm throughput thay vì làm API chết;
- retry và dead-letter rõ ràng.

Chi phí: eventual consistency, duplicate/out-of-order message, vận hành broker, tracing phức tạp và contract evolution.

### Thuật ngữ cần biết

- Exchange định tuyến message; queue giữ message; binding nối exchange–queue bằng routing key.
- Topic exchange match pattern routing key.
- Ack xác nhận xử lý; nack/reject có thể requeue hoặc dead-letter.
- DLQ giữ message xử lý thất bại để điều tra/replay.
- Quorum queue replicate bằng consensus, tăng durability nhưng tốn tài nguyên hơn classic queue.
- Correlation ID nối command, event, log và UI state của cùng một workflow.

### Retry đúng cách

- Chỉ retry lỗi transient: timeout, network, service tạm unavailable.
- Không retry validation/schema/format error.
- Dùng backoff và jitter ở production để tránh retry storm.
- Có max attempts; hết retry chuyển DLQ/error queue.
- Consumer vẫn phải idempotent vì publish/ack failure có thể tạo redelivery.

### Outbox pattern

Vấn đề dual-write:

```text
Save DB thành công -> publish broker thất bại
hoặc
Publish thành công -> DB rollback
```

Outbox ghi business change và message vào cùng DB transaction. Background processor đọc outbox, publish và đánh dấu hoàn tất. Nó không tự tạo exactly-once end-to-end; consumer vẫn cần idempotency. Dự án có reusable outbox cho integration event ở Shared/Screening; dù không phải ownership chính của bạn, cần hiểu vì nó là kiến trúc chung.

## 11. SignalR

SignalR cung cấp realtime server push qua WebSocket và fallback transport. Trong dự án, HTTP submit chỉ xác nhận đã nhận tác vụ; SignalR gửi kết quả completed/failed sau đó.

Các kiểu gửi:

- `Clients.All`: đơn giản nhưng mọi client đều nhận, hiện dự án đang dùng cho JD event.
- User/group: tốt hơn cho multi-tenant và bảo mật; tránh lộ trạng thái job giữa recruiter.

Scale-out nhiều API instance cần Azure SignalR Service hoặc Redis backplane để client ở instance A vẫn nhận event từ instance B. Kết nối có thể rớt, nên frontend vẫn cần refetch trạng thái; SignalR không nên là nguồn dữ liệu duy nhất.

## 12. Testing

### Test pyramid áp dụng trong dự án

- Unit test: validator, handler, mapper, domain method; nhanh và isolate dependency bằng mock.
- Integration test: repository + PostgreSQL Testcontainers; kiểm tra mapping, constraint và SQL behavior thật.
- API/consumer integration: host thật với `WebApplicationFactory`, RabbitMQ container khi cần.
- End-to-end: frontend → API → broker → worker → DB → SignalR; ít hơn vì chậm và dễ flaky.

### Vì sao không chỉ dùng EF InMemory?

EF InMemory không có behavior giống PostgreSQL về transaction, unique constraint, case sensitivity, `ILIKE`, SQL translation và relational mapping. Nó hữu ích cho vài test đơn giản nhưng Testcontainers đáng tin hơn cho repository.

### Cấu trúc test tốt

Arrange–Act–Assert; tên mô tả behavior:

```text
Handle_WhenWeightNameAlreadyExists_ThrowsConflictException
ListAsync_WhenSearchContainsPercent_TreatsPercentAsLiteral
Consume_WhenPayloadIsInvalid_NotifiesFailureAndThrows
```

Test behavior và observable outcome, không test private method. Với mock, verify interaction chỉ khi interaction là contract quan trọng; tránh test dính chặt implementation.

### Những case quan trọng bạn đã/chịu trách nhiệm kiểm tra

- validation đúng/sai và boundary;
- not found/conflict;
- status transition và delete rule;
- duplicate message/idempotency;
- race condition unique constraint;
- cleanup khi publish/processing thất bại;
- consumer completed/failed;
- normalize Markdown/BOM;
- repository tracking/no-tracking và persistence.

Lưu ý tooling hiện tại: project dùng xUnit v3 in-process runner. Trong môi trường kiểm tra, `dotnet test` với Microsoft Testing Platform build được nhưng báo zero discovery; chạy trực tiếp test executable discovery và pass đủ 202 Jobs + 25 Identity tests. Đây là một điểm cấu hình runner/CI nên làm rõ, không phải test logic bị fail.

## 13. SOLID và design principles trong code

- **SRP**: endpoint lo HTTP mapping; handler lo use case; repository lo persistence; validator lo validation.
- **OCP**: abstraction notifier/publisher cho phép thay adapter mà không sửa Application.
- **LSP**: implementation phải giữ contract interface, bao gồm exception/cancellation semantics.
- **ISP**: interface theo capability/use case, không một service khổng lồ.
- **DIP**: Application phụ thuộc interface; Infrastructure implement.
- **DRY**: gom submit workflow vào `JobSubmissionExecutor`, realtime generic vào Shared.
- **YAGNI**: không tạo abstraction khi chưa có nhu cầu; repository cần có giá trị rõ.
- **Rich domain model**: state đổi qua method, private setter bảo vệ invariant.

## 14. Các điểm lead có thể hỏi xoáy

### 14.1. Tại sao check duplicate hai lần?

> Check trước insert giúp trả lỗi sớm và rõ. Nhưng hai request có thể cùng check là false rồi cùng insert. Unique index ở DB bảo vệ invariant; catch unique violation và map 409 xử lý race. Application check là UX, constraint là correctness.

### 14.2. Vì sao `AsNoTracking`?

> Query chỉ đọc không cần snapshot/change tracker. `AsNoTracking` giảm memory và CPU. Với update, em load tracked entity để gọi domain method rồi save.

### 14.3. Tại sao message có thể trùng?

> Broker có thể redeliver nếu consumer xử lý xong nhưng ack bị mất, hoặc producer retry sau khi không nhận confirm. Vì vậy em không dựa vào “message chỉ đến một lần”; em dùng business key `PathWatcher` + unique index và xử lý duplicate như success.

### 14.4. Tại sao không gọi Java bằng HTTP?

> Có thể gọi HTTP nhưng LLM latency dài, timeout và burst sẽ giữ nhiều request/resource. Queue cho backpressure, retry, durability và scale worker riêng. Nếu tác vụ rất nhanh và caller cần kết quả ngay, HTTP sẽ đơn giản hơn.

### 14.5. Vì sao dùng repository nếu EF đã là repository?

> Để bảo vệ module boundary và đặt query theo use case, không phải chỉ bọc CRUD. Em chấp nhận đây là trade-off; nếu repository chỉ forward `DbSet`, em sẽ bỏ abstraction đó.

### 14.6. SignalR event gửi thất bại thì sao?

> Hiện notifier log và swallow để persistence không fail chỉ vì realtime notification. Trade-off là client có thể không nhận event. UI cần reconnect/refetch; nếu notification bắt buộc, nên persist notification/outbox và retry thay vì best-effort.

### 14.7. Xóa directory và publish có atomic không?

> Không, filesystem và broker không chung transaction. Code hiện dùng compensation: publish fail thì cleanup directory. Vẫn có crash window. Cải thiện bằng state machine/job record trước, idempotent cleanup/reconciliation background job hoặc đổi ownership để worker tự tạo resource sau khi nhận command.

### 14.8. Có điểm nào bạn muốn cải thiện?

Câu trả lời tốt, không tự hạ thấp dự án:

1. Gửi SignalR theo user/group thay `Clients.All`.
2. Thêm optimistic concurrency/ETag cho update.
3. Tách transaction boundary rõ hơn thay vì repository tự save từng method.
4. Thêm stable ordering/keyset pagination cho dữ liệu lớn.
5. Thêm end-to-end tracing qua HTTP–RabbitMQ–Java bằng correlation ID/OpenTelemetry.
6. Thêm contract/integration test cho message version compatibility.
7. Harden upload/path validation, request limit và malware scanning.
8. Thêm reconciliation cho filesystem/broker compensation window.

## 15. Câu hỏi nhanh và đáp án ngắn

1. **Kestrel là gì?** Web server cross-platform của ASP.NET Core, nhận HTTP request; production thường đứng sau reverse proxy/load balancer.
2. **`IEnumerable` và `IQueryable`?** `IEnumerable` thực thi logic trong .NET trên dữ liệu đã materialize; `IQueryable` xây expression để provider như EF dịch sang SQL. Chuyển sang `IEnumerable` quá sớm có thể kéo toàn bộ dữ liệu về memory.
3. **`FirstOrDefault` và `SingleOrDefault`?** First lấy phần tử đầu; Single yêu cầu tối đa một và throw nếu nhiều. Với unique invariant, `SingleOrDefault` thể hiện kỳ vọng nhưng DB vẫn cần unique constraint.
4. **`SaveChanges` trả gì?** Số state entry bị ghi, không phải lúc nào đồng nghĩa số business entity.
5. **Scoped service trong background worker?** Tạo scope mỗi iteration/message bằng `IServiceScopeFactory`.
6. **401 và 403?** 401 chưa xác thực/token không hợp lệ; 403 đã xác thực nhưng không đủ quyền.
7. **CORS chống được Postman/curl không?** Không. CORS do browser enforce; API vẫn cần auth/authz.
8. **JWT có mã hóa payload không?** Thông thường không; JWT signed chỉ base64url + signature, ai có token có thể đọc claim. Không đặt secret/PII nhạy cảm trong payload.
9. **DI giúp test thế nào?** Handler phụ thuộc interface, test thay dependency bằng fake/mock.
10. **`CancellationToken` có tự rollback DB không?** Nếu hủy trong operation, provider throw; transaction chưa commit có thể rollback. Không nên giả định mọi external side effect tự rollback.
11. **`Include` luôn tốt?** Không; có thể tải dư và Cartesian explosion. Projection thường tốt cho read DTO.
12. **Index mọi column?** Không; write/storage cost tăng. Chọn theo query pattern và đo bằng execution plan.
13. **`async void`?** Chỉ nên dùng event handler; không await được và exception khó kiểm soát. API/service dùng `Task`/`ValueTask`.
14. **Transient fault?** Lỗi tạm thời có khả năng thành công khi thử lại, như timeout/network; khác validation lỗi vĩnh viễn.
15. **Exactly-once có thật không?** Khó bảo đảm end-to-end; thường đạt hiệu quả tương đương bằng at-least-once + idempotent consumer + deduplication.
16. **Optimistic vs pessimistic lock?** Optimistic cho phép concurrent read/update rồi phát hiện conflict bằng version; pessimistic khóa sớm, giảm conflict nhưng tăng blocking/deadlock.
17. **DTO mapping ở đâu?** Gần application/use case để domain không phụ thuộc transport; query có thể projection thẳng DTO khi phù hợp.
18. **Domain event vs integration event?** Domain event trong boundary/process; integration event vượt module/service, cần stable contract và reliable delivery.
19. **Health check và readiness?** Liveness cho biết process sống; readiness cho biết sẵn sàng nhận traffic, phụ thuộc DB/broker cần thiết.
20. **OpenTelemetry giúp gì?** Trace, metric, log correlation qua service; đặc biệt hữu ích cho workflow HTTP → RabbitMQ → worker.

## 16. Ba câu chuyện STAR nên chuẩn bị

### STAR 1 — Duplicate JD và race condition

- **Situation:** RabbitMQ có thể redeliver; hai message cùng `PathWatcher` có thể xử lý gần đồng thời.
- **Task:** Không tạo duplicate job và vẫn trả trạng thái đúng cho UI.
- **Action:** Check existing theo business key, đặt unique index, catch `DbUpdateException`, đọc record thắng race, xem duplicate là idempotent success; thêm test consumer/service.
- **Result:** Luồng an toàn hơn với at-least-once delivery, tránh duplicate data và lỗi UI.

### STAR 2 — Weight Markdown validation

- **Situation:** Weight profile do người dùng import, format có nhiều biến thể và dữ liệu sai ảnh hưởng scoring.
- **Task:** Validate chặt nhưng trả lỗi dễ hiểu.
- **Action:** Dùng Markdig parse AST, nhận diện heading/table/column, hỗ trợ decimal và percentage, cấm mixed unit, chuẩn hóa BOM/whitespace, test boundary và malformed input.
- **Result:** Dữ liệu vào nhất quán, lỗi được chặn trước persistence, code ít phụ thuộc regex mong manh.

### STAR 3 — Realtime sau xử lý AI

- **Situation:** Parse JD mất lâu, HTTP request không nên chờ.
- **Task:** API responsive nhưng frontend vẫn biết completed/failed.
- **Action:** Trả 202, publish command, consumer persist result, dùng correlation ID và SignalR notify; refactor notifier thành shared abstraction.
- **Result:** Tách request path khỏi AI latency và cho UI cập nhật realtime; module khác có thể tái sử dụng notifier.

Khi kể STAR, bổ sung con số thật nếu bạn có: số test, coverage, bug đã giảm, thời gian response hoặc review comment đã giải quyết. Không tự bịa metric.

## 17. Câu hỏi nên hỏi ngược team lead

- Với level chính thức, anh/chị kỳ vọng em tự chủ đến mức nào trong design, implementation và production support?
- Trong 3 tháng tới, kỹ năng kỹ thuật nào của em cần ưu tiên để tạo tác động lớn nhất cho team?
- Team đánh giá chất lượng backend qua những tiêu chí nào: correctness, test, observability, performance hay delivery speed?
- Phần nào trong dự án anh/chị muốn em tiếp tục ownership nếu lên chính thức?
- Em có thể tham gia nhiều hơn vào design review, incident/debug production hoặc deployment không?

## 18. Kế hoạch ôn 7 ngày

### Ngày 1 — Câu chuyện dự án

- Vẽ lại architecture không nhìn tài liệu.
- Nói bài 60 giây và 3 phút, tự ghi âm.
- Chốt chính xác ownership và 3 STAR stories.

### Ngày 2 — ASP.NET Core/Web API

- Pipeline, middleware order, DI lifetime, configuration, async/cancellation.
- HTTP verb/status, ProblemDetails, auth vs authorization, CORS.
- Đọc `Program.cs`, DI và 3 endpoint tiêu biểu.

### Ngày 3 — EF Core/PostgreSQL

- Tracking, projection, Include/N+1, migration, index, transaction, concurrency.
- Đọc `JobsDbContext`, entity configurations và repositories.
- Tự giải thích SQL dự kiến của list jobs/weights.

### Ngày 4 — Jobs end-to-end

- Trace submit success, failed, duplicate và retry path.
- Đọc executor, consumers, result service, SignalR notifier.
- Trả lời vì sao 202, vì sao idempotent, vì sao unique constraint.

### Ngày 5 — Weights, Identity và security

- Trace CRUD/validator/repository.
- Ôn token/claim/role/refresh token/password hashing.
- Ôn upload security và data privacy.

### Ngày 6 — Testing và system design

- Unit vs integration vs E2E, mock vs Testcontainers.
- RabbitMQ, DLQ, outbox, eventual consistency, retry.
- Chuẩn bị 3 cải tiến có trade-off rõ.

### Ngày 7 — Mock interview

- 15 phút project walkthrough.
- 30 phút 20 câu hỏi nhanh.
- 15 phút behavioral/STAR.
- Sửa câu trả lời dài dòng: mỗi câu mở bằng kết luận, sau đó lý do và ví dụ dự án.

## 19. Checklist trước buổi interview

- [ ] Tôi nói được vai trò của từng service mà không nhìn sơ đồ.
- [ ] Tôi trace được submit JD từ HTTP đến SignalR.
- [ ] Tôi giải thích được 202, idempotency, retry và DLQ.
- [ ] Tôi phân biệt tracking/no-tracking, projection/Include và unit/integration test.
- [ ] Tôi giải thích được unique constraint chống race condition.
- [ ] Tôi phân biệt authentication/authorization, 401/403 và access/refresh token.
- [ ] Tôi có 3 STAR stories, mỗi story dưới 3 phút.
- [ ] Tôi nêu được 3 điểm cải thiện mà không phủ nhận giá trị hiện tại.
- [ ] Tôi biết phần nào là đóng góp cá nhân và phần nào là công sức của team.
- [ ] Tôi có 2–3 câu hỏi hỏi ngược team lead.

## 20. Tóm tắt để nhớ ngay trước khi vào phòng

> Ownership chính của tôi: Jobs, Weights, JD async result integration, SignalR và test Jobs/Weights/Identity.
>
> Giá trị kỹ thuật nổi bật: tách AI latency khỏi HTTP bằng RabbitMQ; bảo đảm consumer idempotent; chống race bằng DB constraint; dùng rich domain model/repository/CQRS; query read-only bằng `AsNoTracking` và projection; validation Markdown có cấu trúc; test cả logic và PostgreSQL behavior thật.
>
> Điều tôi đã tiến bộ: từ làm happy path sang suy nghĩ về failure path, duplicate delivery, cleanup, concurrency, contract và maintainability.
>
> Điều tôi muốn phát triển tiếp: production observability, security/authorization sâu hơn, concurrency control và ownership end-to-end.
