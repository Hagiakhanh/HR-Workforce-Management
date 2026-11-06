# Ví dụ từ dự án để luyện phỏng vấn — Hà Gia Khánh

Đối chiếu ngày 06/11/2026. Phạm vi: lịch sử có trên nhánh `dev` của remote `origin`, từ khi chuyển sang cấu trúc hiện tại.

Tài liệu gồm hai phần: mục 1–9 là bằng chứng PR và trải nghiệm để luyện; mục 10–15 là lý thuyết, cách áp dụng và câu trả lời mẫu theo yêu cầu HR Manager. Có thể đọc toàn bộ trong file này.

Đi nhanh đến: [Cách chuẩn bị theo HR](#10-cach-on-theo-yeu-cau-hr) · [SDLC](#11-sdlc) · [SOLID](#12-solid) · [Clean Code](#13-clean-code) · [Agile và Scrum](#14-agile-va-scrum) · [Bản ôn nhanh](#15-ban-on-nhanh).

## 1. Phạm vi đã kiểm tra

- Đã kiểm tra trực tiếp bằng `git ls-remote`: đầu nhánh `dev` trên GitHub là `4c379935122776913329be149c070d8a411693fa`, khớp `origin/dev` và `HEAD` trong workspace.
- Cấu trúc hiện tại hình thành qua nhiều commit. `f5c557a6` tách Shared thành Application/Domain/Infrastructure: ngày tác giả 30/09, ngày ghi commit 01/10. `b5a196ce` ngày 01/10 chuyển Identity vào các project Domain/Application/Infrastructure và Mediator. Chọn `f5c557a6` làm mốc cấu trúc để lọc lịch sử tiếp theo.
- Các ví dụ chính bên dưới dùng commit có tác giả Git là `Ha Gia Khanh`. Đã kiểm tra chúng nằm sau mốc cấu trúc, có trong lịch sử `origin/dev`, và thuộc nhánh được đưa vào PR tương ứng, chưa có ở base của PR đó.
- Không lấy các PR của cấu trúc cũ từ tháng 8 hoặc đầu tháng 9 làm ví dụ trong tài liệu này. PR #281 chỉ được dùng ở phần chuyển Identity sang cấu trúc mới, cụ thể `b5a196ce`.
- Số PR và tiêu đề được đối chiếu từ merge commit; chưa đọc được nội dung review/comment trên trang PR. Không xác định người review, câu góp ý nguyên văn hoặc nội dung trao đổi ngoài Git.
- Đã đọc code và nội dung test; không chạy build/test trong lần rà soát phục vụ phỏng vấn này. Vì vậy, “có test cho trường hợp X” không có nghĩa là xác nhận test vừa chạy pass.

Đây là bằng chứng về thay đổi trong repository. Khi luyện lời kể cá nhân, bạn chỉ thêm lý do, cuộc trao đổi hoặc kết quả thực tế mà mình nhớ và xác nhận được.

## 2. Các PR liên quan sau khi chuyển cấu trúc

| PR | Mốc merge trong lịch sử | Phần có thể dùng để kể |
|---|---|---|
| [#281 — Authentication with OpenIddict](https://github.com/bbv-thanhnguyen/ypp-2026-aihr/pull/281) | 01/10, `38be226d` | Commit `b5a196ce` chuyển Identity sang cấu trúc mới: hiểu layer, trách nhiệm và cách áp dụng training. |
| [#293 — Identity database initializer](https://github.com/bbv-thanhnguyen/ypp-2026-aihr/pull/293) | 05/10, `c90d75fe` | Tách initializer và xác định schema: tổ chức trách nhiệm khi khởi tạo database. |
| [#291 — Import JD US001](https://github.com/bbv-thanhnguyen/ypp-2026-aihr/pull/291) | 08/10, `591b631d` | Tách messaging qua interface; hai đầu vào file/Markdown dùng chung bước submit; sửa theo review. |
| [#301 — Manage jobs](https://github.com/bbv-thanhnguyen/ypp-2026-aihr/pull/301) | 19/10, `8669193d` | Endpoint quản lý job, repository, domain behavior và test. |
| [#316 — Consume parsed JD](https://github.com/bbv-thanhnguyen/ypp-2026-aihr/pull/316) | 26/10, `55081b72` | Nhận kết quả bất đồng bộ, xử lý bản tin trùng/lỗi, lưu Job và thông báo frontend. |
| [#319 — Manage weight](https://github.com/bbv-thanhnguyen/ypp-2026-aihr/pull/319) | 28/10, `b854dbba` | Validation theo acceptance criteria, chuẩn hóa nội dung, xử lý xung đột import và test. |
| [#328 — Job screening endpoints and tests](https://github.com/bbv-thanhnguyen/ypp-2026-aihr/pull/328) | 29/10, `6b8477b9` | Nhận PR feedback, thu gọn UserRepository, điều chỉnh endpoint và cập nhật test. |

Ngày trong bảng là ngày tác giả của merge commit. Đây là các mốc trong lịch sử nhánh, không phải lịch bắt đầu/kết thúc task hay số ngày bạn làm việc.

## 3. Câu chuyện mạnh nhất cho việc tiếp nhận góp ý

**PR #328, commit `a74a223a`, ngày 29/10: `address PR feedback on endpoints and user repo`.**

### Trước và sau

Trước thay đổi, `IUserRepository` có tham số `trackChanges` cho hai hàm đọc và có cả `UpdateAsync`. Sau thay đổi, các thành phần đó được bỏ; handler đăng ký cũng bỏ bước tự chuẩn hóa email bị lặp, trong khi repository/domain đảm nhiệm phần liên quan. Test được sửa theo thay đổi contract. Endpoint screening-context được bổ sung role và mô tả response.

Commit tiếp theo `d0491fd3` bổ sung `AsNoTracking()` cho hai hàm đọc. Cần phân biệt đây là bước tiếp theo, không gộp mọi thay đổi vào một commit.

Code hiện tại: [IUserRepository](backend/dotnet/AI@HR/src/Modules/Identity/AI.HR.Identity.Application/Common/Interfaces/IUserRepository.cs), [UserRepository](backend/dotnet/AI@HR/src/Modules/Identity/AI.HR.Identity.Infrastructure/Persistence/Repositories/UserRepository.cs), [RegisterUserCommand](backend/dotnet/AI@HR/src/Modules/Identity/AI.HR.Identity.Application/Users/Commands/RegisterUserCommand.cs).

### Bản trả lời để luyện, khoảng 60–90 giây

> Em lấy ví dụ ở PR về endpoints và tests. Trong quá trình review, em có chỉnh lại UserRepository. Phiên bản trước có tham số và phương thức mà sau đó em đã bỏ để interface phù hợp hơn với nhu cầu đang dùng.
>
> Em điều chỉnh các chỗ gọi, bỏ bước chuẩn hóa email bị lặp trong handler đăng ký và cập nhật test theo contract mới. Sau đó em cũng bổ sung AsNoTracking cho các truy vấn chỉ đọc.
>
> Qua việc này, em học được rằng cần hiểu lý do của góp ý và xem cả các chỗ bị ảnh hưởng khi sửa. Với em, tiếp nhận feedback thể hiện ở việc điều chỉnh code và kiểm tra hành vi liên quan.

Kết quả có thể xác nhận: interface và implementation được thu gọn, test thay đổi tương ứng, code nằm trong lịch sử PR #328. Chưa có căn cứ để nói số lỗi giảm bao nhiêu hoặc mentor đánh giá bạn thế nào.

Bản tiếng Anh ngắn:

> In one PR, I updated the user repository after review feedback. I removed extra options and a method, updated the callers, and adjusted the tests. I learned to understand the reason for feedback and check the impact of my changes.

### Câu có thể bị hỏi tiếp

- **Vì sao bỏ `trackChanges` ở UserRepository nhưng WeightRepository vẫn có?** Nhìn nhu cầu sử dụng từng repository. Weight có luồng update; không có quy tắc bắt buộc mọi repository giống nhau. Đọc lại các caller trước khi giải thích chi tiết.
- **Ai góp ý và góp ý nguyên văn thế nào?** Git hiện chỉ xác nhận commit nói xử lý PR feedback. Chỉ nêu người hoặc câu nói khi nhớ được.
- **Em kiểm tra ảnh hưởng thế nào?** Chỉ vào những test thay đổi trong commit và giải thích chúng kiểm tra gì; không khẳng định đã tự chạy một lệnh cụ thể nếu chưa nhớ.

## 4. Ví dụ về yêu cầu nghiệp vụ và chất lượng: weight validation

**PR #319, commit `49e7aa42`, ngày 21/10: `fix validator follow ac in user story`.** AC là acceptance criteria — điều kiện để đánh giá yêu cầu đã được đáp ứng.

Code: [WeightValidator](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Application/Weights/Validators/WeightValidator.cs). Test: [WeightValidatorTests](backend/dotnet/AI@HR/tests/Modules/Jobs/AI.HR.Jobs.Application.Tests/Weights/Validators/WeightValidatorTests.cs).

### Điều cụ thể để nhớ

- Nội dung cần có tiêu đề và phần Tier 1 — Must-Have với bảng trọng số.
- Trọng số cần hợp lệ, dương và tổng bằng 1 sau khi quy đổi.
- `0.60 + 0.40` hoặc `60% + 40%` có thể biểu diễn một tổng hợp lệ.
- `1%` phải được hiểu là `0.01`. Một file chỉ có một tiêu chí trọng số `1%` không đạt điều kiện tổng 100%.
- Không trộn `50%` và `0.50` trong cùng bảng; định dạng sai như `5%0` phải được báo lỗi.

Diff cũ cho thấy logic từng bỏ dấu `%` rồi chia 100 khi số lớn hơn 1. Với `1%`, cách suy đoán đó có thể cho kết quả 1 thay vì 0.01. Code sau thay đổi xét định dạng phần trăm rõ ràng hơn; source có test riêng cho `1%`, `1% + 99%`, sai vị trí dấu `%`, thiếu dữ liệu và trộn đơn vị.

Các quy tắc trên được đọc từ implementation/test hiện tại và diff. Chưa có nội dung user story gốc để trích nguyên văn toàn bộ AC hoặc xác nhận ai quyết định từng quy tắc.

### Bản trả lời để luyện

> Với phần weight configuration, em có một thay đổi sửa validator theo acceptance criteria của user story. Yêu cầu cần được thể hiện thành các rule cụ thể như cấu trúc file, giá trị trọng số và tổng trọng số.
>
> Một trường hợp đáng chú ý là 1% phải được hiểu là 0.01. Em điều chỉnh cách nhận biết đơn vị và bổ sung các trường hợp kiểm tra như tổng không hợp lệ, định dạng phần trăm sai hoặc trộn đơn vị.
>
> Bài học của em là cần kiểm tra ý nghĩa nghiệp vụ của dữ liệu, đồng thời biến các trường hợp biên thành test để bảo vệ hành vi khi sửa code.

Phù hợp với câu hỏi: em làm đúng yêu cầu thế nào, chất lượng thể hiện ở đâu, em học được gì, SDLC được áp dụng ra sao. Để kể một lần trao đổi trực tiếp với PO vẫn cần trí nhớ hoặc bằng chứng về cuộc trao đổi đó.

## 5. Ví dụ về problem solving: import weight cùng tên

**PR #319, commit `3f132294`, ngày 27/10: `handle import concurrency, content normalization, and add tests`.**

Code: [ImportWeightCommand](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Application/Weights/Commands/ImportWeight/ImportWeightCommand.cs), [WeightConfiguration](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Infrastructure/Persistence/Configurations/WeightConfiguration.cs), [DbUpdateExceptionExtensions](backend/dotnet/AI@HR/src/Shared/AI.HR.Shared.Application/Common/Exceptions/DbUpdateExceptionExtensions.cs).

### Vấn đề và cách xử lý

Hai request cùng tên có thể cùng kiểm tra `ExistsByNameAsync` và đều nhận “chưa tồn tại” trước khi một request lưu xong. Vì vậy, bước kiểm tra trước không tự bảo đảm dữ liệu duy nhất.

Code hiện tại có unique index trên tên weight. Khi thao tác lưu phát sinh unique violation, handler chuyển exception thành `ConflictException`; HTTP exception handler ánh xạ loại này sang 409. Commit còn chuẩn hóa nội dung bằng `Weight.NormalizeContent` và bổ sung test.

Test liên quan: `Handle_WhenConcurrentImportCausesUniqueViolation_ShouldThrowConflictException` trong [ImportWeightCommandHandlerTests](backend/dotnet/AI@HR/tests/Modules/Jobs/AI.HR.Jobs.Application.Tests/Weights/Commands/ImportWeight/ImportWeightCommandHandlerTests.cs).

Test này dùng mock để mô phỏng unique violation khi lưu; nó không chạy hai request đồng thời với database thật. Các repository test dùng PostgreSQL Testcontainers là bằng chứng khác, chủ yếu kiểm tra search, paging, update và delete; không gán cho chúng khả năng kiểm tra concurrency nếu không có case tương ứng.

### Bản trả lời để luyện

> Một vấn đề kỹ thuật em có xử lý là import weight cùng tên. Việc kiểm tra tên trước khi lưu chưa đủ, vì hai request có thể cùng thấy tên chưa tồn tại.
>
> Em bổ sung xử lý unique violation khi lưu và chuyển thành lỗi conflict phù hợp cho phía gọi. Trong code cũng có ràng buộc unique ở database và unit test mô phỏng nhánh phát sinh lỗi đó.
>
> Qua đây em hiểu cần xét cả thời điểm dữ liệu được lưu và những tình huống ngoài luồng thành công. Test cũng cần kiểm tra đúng hành vi lỗi, thay vì chỉ kiểm tra import thành công.

Không khẳng định đây là một sự cố production đã xảy ra hoặc có một khách hàng gặp lỗi. Bằng chứng hiện có là thay đổi code nhằm xử lý trường hợp này.

## 6. Ví dụ rõ nhất cho DIP: tách Application khỏi messaging

**PR #291, commit `dea1a561`, ngày 03/10: `decouple application layer from shared infrastructure`.**

Diff xác nhận ba thay đổi:

1. Bỏ project reference từ Jobs.Application đến Shared.Infrastructure.
2. Định nghĩa `IJdCommandPublisher` ở Jobs.Application; handler dùng abstraction này.
3. Đưa chi tiết gửi message vào `JdCommandPublisher` ở Jobs.Infrastructure, rồi đăng ký implementation qua DI.

Code hiện tại: [IJdCommandPublisher](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Application/Common/Interfaces/IJdCommandPublisher.cs), [JdCommandPublisher](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Infrastructure/Messaging/JdCommandPublisher.cs), [Jobs Infrastructure DI](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Infrastructure/DependencyInjection.cs).

### Bản trả lời để luyện

> Em hiểu Dependency Inversion là để phần xử lý cấp cao phụ thuộc vào abstraction, còn chi tiết triển khai tuân theo abstraction đó. Trong luồng import JD, em có một thay đổi tách Application khỏi Shared.Infrastructure.
>
> Em tạo IJdCommandPublisher ở Application và đưa chi tiết gửi message sang implementation ở Infrastructure. Nhờ đó, phần use case diễn đạt nhu cầu gửi lệnh parse JD mà không cần biết exchange hoặc routing key cụ thể. Dependency injection kết nối implementation khi ứng dụng chạy.

Lợi ích là giảm sự gắn chặt ở boundary này và dễ thay dependency trong test. Không khẳng định thay broker sẽ không cần sửa gì, hoặc toàn bộ Application hoàn toàn độc lập framework: project hiện vẫn có dependency thư viện như EF Core.

## 7. Ví dụ bổ sung nếu muốn trao đổi sâu

### Tránh lặp logic submit JD

PR #291, commit `f0d4c5c2` ngày 07/10 có tiêu đề nêu rõ refactor và resolve review comments. Hai handler nhận file/Markdown cùng dùng [JobSubmissionExecutor](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Application/Jobs/Commands/JobSubmissionExecutor.cs) cho các bước chung: kiểm tra directory, tạo directory và gửi command. Mỗi handler giữ bước đọc đầu vào tương ứng.

Có thể dùng cho SRP, DRY, Clean Code và tiếp nhận feedback. Cách giải thích: gom đúng một luồng nghiệp vụ chung để thay đổi nhất quán, đồng thời giữ phần đọc input riêng ở từng handler.

### Bảo vệ trạng thái entity

PR #316, commit `243cf310` ngày 20/10 chuyển `Job.Criteria` từ collection có thể chỉnh bên ngoài thành collection chỉ đọc, giữ danh sách bên trong entity và tính sort order trong hành vi thêm criterion.

Code: [Job](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Domain/Entities/Job.cs). Có thể dùng cho encapsulation và cách tổ chức domain behavior. Encapsulation là kiến thức thiết kế liên quan, không phải một chữ riêng của SOLID.

### Luồng nhận kết quả JD bất đồng bộ

PR #316 có các commit `65565d48`, `80c4c82c`, `ddcbc371`. Code hiện tại ở [JdProcessingResultService](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Application/Jobs/Services/JdProcessingResultService.cs) và [ParseJdConsumer](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Infrastructure/Messaging/ParseJdConsumer.cs) xử lý lưu kết quả, tránh tạo thêm Job khi đã có PathWatcher và thông báo trạng thái.

Source có test cho payload hợp lệ, thiếu criteria, lỗi lưu, bản ghi đã tồn tại và nhánh insert gặp xung đột. Đây là ví dụ để nói về chất lượng integration. Không diễn đạt thành bảo đảm xử lý exactly-once hoặc chống mọi lỗi bất đồng bộ.

## 8. Ghép ví dụ với các câu hỏi culture fit

| Câu hỏi | Ví dụ nên ưu tiên | Ý cần làm rõ |
|---|---|---|
| Điểm mạnh của em là gì? | PR #328, `a74a223a` | Tiếp nhận góp ý và điều chỉnh code/caller/test. |
| Kể một khó khăn kỹ thuật. | PR #319, `3f132294` | Kiểm tra trước khi lưu chưa đủ khi có concurrency. |
| Em bảo đảm chất lượng thế nào? | PR #319, `49e7aa42` | Dữ liệu biên như 1%, lỗi định dạng và test tương ứng. |
| Em ứng dụng SOLID thế nào? | PR #291, `dea1a561` | Vị trí abstraction, chiều dependency, implementation và DI. |
| Em hiểu Clean Code ra sao? | PR #291, `f0d4c5c2` | Logic submit chung và input khác nhau có trách nhiệm rõ. |
| Em chuyển yêu cầu thành code thế nào? | PR #319, `49e7aa42` | AC → validation rule → test hành vi. |
| Em làm việc theo vòng phản hồi thế nào? | PR #328 hoặc #291 | Implementation → review → chỉnh sửa; Git không chứng minh đầy đủ Scrum. |
| Em phối hợp với team thế nào? | PR #316 + slide 12/18 | Ranh giới .NET/Java, contract và notification; chỉ kể cuộc trao đổi thực tế còn nhớ. |

Với câu hỏi về khách hàng, có thể nói đến kết quả người dùng cần và cách validate dữ liệu. Chưa có bằng chứng để nhận mình trực tiếp trao đổi hoặc quản lý khách hàng bên ngoài.

## 9. Cách luyện từ bằng chứng này

Ưu tiên bốn ví dụ ở mục 3–6. Mỗi ví dụ ghi năm ý ngắn: nhu cầu/vấn đề; phần mình sửa; lý do cách sửa phù hợp; test hoặc code thể hiện kết quả; điều mình học được.

Không cần nhớ hash khi nói chuyện. Có thể gọi tên feature: “Ở phần weight configuration…” hoặc “Trong PR chỉnh UserRepository…”. Hash và link phục vụ việc xem lại trước phỏng vấn.

Phần lý thuyết và cách ứng dụng được trình bày ngay bên dưới. [Tài liệu chuẩn bị chính](BBV_Culture_Fit_Prep_Ha_Gia_Khanh.md) vẫn có các câu general bằng tiếng Anh và kế hoạch luyện phỏng vấn.

<a id="10-cach-on-theo-yeu-cau-hr"></a>

## 10. Cách ôn theo yêu cầu HR Manager

Bạn đã hỏi nên chuẩn bị khái niệm hay cách áp dụng và HR trả lời:

> Khái niệm, và thực tế em ứng dụng khái niệm đó trong quá trình internship thế nào.

Vì vậy, mỗi chủ đề nên trả lời theo bốn ý:

1. **Khái niệm:** Nó là gì, giải quyết vấn đề nào?
2. **Cách hiểu:** Những thành phần hoặc nguyên tắc chính là gì?
3. **Ứng dụng:** Trong internship, mình đã làm phần việc nào liên quan?
4. **Bài học:** Cách áp dụng đó mang lại gì, có giới hạn hoặc điều cần cải thiện nào?

Ví dụ một câu trả lời có chiều sâu vừa đủ:

> Em hiểu Dependency Inversion là nguyên tắc tổ chức dependency qua abstraction. Ở import JD, em có thay đổi để Application dùng IJdCommandPublisher, còn Infrastructure giữ chi tiết gửi message. Việc đó giúp use case bớt gắn chặt với cấu hình messaging. Em cũng hiểu đây là một boundary cụ thể; không có nghĩa toàn bộ project đã độc lập mọi framework.

Chuẩn bị giải thích được tên class và luồng code liên quan để trả lời tiếp. Không cần đọc nguyên code trong phần trả lời mở đầu. Các câu trả lời mẫu dưới đây dùng dữ liệu đã đối chiếu ở mục 1–9; đọc lại để bảo đảm bạn hiểu từng câu.

<a id="11-sdlc"></a>

## 11. SDLC — Software Development Life Cycle

### 11.1. Khái niệm và mục đích

SDLC là vòng đời phát triển phần mềm: tổ chức công việc từ nhu cầu ban đầu đến xây dựng, đưa vào sử dụng và duy trì. Mục đích là tạo sản phẩm đáp ứng nhu cầu, kiểm soát chất lượng và sử dụng nguồn lực phù hợp. Tùy mô hình, các hoạt động có thể tuần tự hoặc lặp/chồng lấn. Nguồn: [IBM — SDLC](https://www.ibm.com/think/topics/sdlc).

### 11.2. Các hoạt động chính

| Hoạt động | Câu hỏi cần giải quyết | Đầu ra thường gặp |
|---|---|---|
| Planning | Làm gì, ưu tiên gì, nguồn lực/rủi ro nào? | Phạm vi và kế hoạch ban đầu. |
| Requirements analysis | Người dùng cần kết quả nào? | Yêu cầu, use case, acceptance criteria. |
| Design | Tổ chức giải pháp thế nào? | Thiết kế dữ liệu, API và thành phần. |
| Implementation | Hiện thực hành vi thế nào? | Code và thay đổi cấu hình. |
| Testing | Hành vi có đúng kỳ vọng không? | Test, kết quả kiểm tra, lỗi cần xử lý. |
| Deployment | Đưa phiên bản vào môi trường đích thế nào? | Phiên bản được triển khai. |
| Maintenance | Duy trì và cải thiện sau sử dụng thế nào? | Sửa lỗi, cập nhật và phản hồi. |

Đây là cách nhóm hoạt động phổ biến; tên hoặc số bước có thể khác giữa các tổ chức. Tham khảo thêm [AWS — SDLC](https://aws.amazon.com/what-is/sdlc/).

### 11.3. Những điểm cần phân biệt

- **Requirement và giải pháp:** “Trọng số phải có tổng hợp lệ” mô tả hành vi cần đạt; chọn parser Markdown hoặc class nào là quyết định implementation.
- **Acceptance criteria — AC:** Điều kiện để đánh giá một yêu cầu cụ thể đã được đáp ứng. Ví dụ từ code hiện tại: file weight có cấu trúc phù hợp và tổng trọng số sau quy đổi bằng 1.
- **Review và testing:** Review xem xét giải pháp/code; test thực thi kiểm tra hành vi. Hai việc hỗ trợ nhau.
- **SDLC và Agile:** SDLC mô tả các hoạt động của vòng đời; Agile định hướng cách tổ chức công việc và phản hồi. Team có thể thực hiện nhiều hoạt động SDLC trong một vòng phát triển ngắn.

Ví dụ của bạn cho thấy việc xem lại yêu cầu và chỉnh implementation có thể diễn ra sau một phiên bản code ban đầu. Không nên hình dung rằng phân tích, thiết kế và kiểm tra chỉ được làm đúng một lần.

### 11.4. Các loại test liên quan đến dự án

**Unit test** kiểm tra một đơn vị hành vi trong phạm vi hẹp, có thể thay dependency bên ngoài bằng mock. **Integration test** kiểm tra các thành phần phối hợp, thường có database hoặc hạ tầng thật. Nguồn: [Microsoft — Testing in .NET](https://learn.microsoft.com/en-us/dotnet/core/testing/).

Ví dụ thực tế để phân biệt:

| Ví dụ trong repository | Điều được kiểm tra |
|---|---|
| `WeightValidatorTests` | Quy tắc đọc và kiểm tra nội dung weight. |
| `ImportWeightCommandHandlerTests` với repository mock | Handler phản ứng thế nào khi có dữ liệu hợp lệ hoặc lỗi khi lưu. |
| `WeightRepositoryTests` dùng PostgreSQL Testcontainers | Query/update/delete hoạt động với PostgreSQL thế nào. |

Mock ném unique violation giúp kiểm tra nhánh xử lý lỗi của handler; không tự chứng minh hai request đồng thời đã được kiểm thử với database thật. Chất lượng test nằm ở hành vi nó kiểm tra, không chỉ ở số lượng test.

### 11.5. Bạn đã ứng dụng SDLC thế nào?

Bằng chứng dưới đây đến từ nhiều PR, không phải một feature do bạn tự làm toàn bộ từ đầu đến production:

| Hoạt động | Ví dụ cá nhân đã tìm thấy |
|---|---|
| Chuyển yêu cầu thành rule | PR #319, `49e7aa42`: chỉnh validator theo AC của user story. |
| Thiết kế dependency | PR #291, `dea1a561`: Application dùng abstraction gửi command. |
| Implementation | Handler, validator, repository và endpoint của import JD/manage weight. |
| Kiểm tra hành vi | Test cho `1%`, định dạng sai, tổng sai và unique violation. |
| Điều chỉnh qua review | PR #328, `a74a223a`: sửa repository, caller và test. |

Repository có CI build/test và SonarQube cho PR vào dev. Có thể nói mình làm việc trong quy trình đó; chưa có căn cứ nhận mình là tác giả pipeline hoặc xác nhận một lần chạy CI cụ thể. Deployment, monitoring và production maintenance vẫn là phần cần phân biệt với công việc đã trực tiếp làm.

### 11.6. Câu trả lời mẫu

> Em hiểu SDLC là vòng đời từ nhu cầu đến thiết kế, implementation, kiểm thử, triển khai và duy trì phần mềm. Trong internship, em tham gia rõ hơn ở chuyển yêu cầu thành code, kiểm thử và điều chỉnh qua review.
>
> Ví dụ ở weight configuration, em sửa validator theo AC và bổ sung test cho đơn vị phần trăm, tổng trọng số và dữ liệu không hợp lệ. Ở import JD, em có thay đổi tách dependency giữa Application và Infrastructure. Qua đó em thấy các hoạt động này liên quan và có thể lặp lại khi nhận feedback. Deployment và vận hành production là phần em muốn học sâu thêm.

**Nếu hỏi tiếp “Vì sao cần SDLC?”:** Để mọi người cùng hiểu nhu cầu, cách triển khai và cách kiểm tra, đồng thời nhìn được công việc vượt ra ngoài coding. Có thể lấy trường hợp weight: API lưu được file chưa đủ; nội dung và ý nghĩa trọng số cũng phải đúng.

<a id="12-solid"></a>

## 12. SOLID — năm nguyên tắc thiết kế

### 12.1. Mục đích chung

SOLID giúp suy nghĩ về trách nhiệm, khả năng mở rộng và hợp đồng giữa các thành phần. Năm nguyên tắc gồm:

| Chữ | Nguyên tắc | Khái niệm |
|---|---|---|
| S | Single Responsibility | Một thành phần có một lý do chính để thay đổi; tách các trách nhiệm thay đổi vì lý do khác nhau. |
| O | Open/Closed | Cho phép mở rộng hành vi mà hạn chế sửa phần ổn định. |
| L | Liskov Substitution | Kiểu/implementation thay thế phải giữ hợp đồng hành vi mà client trông đợi. |
| I | Interface Segregation | Client không bị buộc phụ thuộc các phương thức không cần dùng. |
| D | Dependency Inversion | Chính sách cấp cao và chi tiết phụ thuộc abstraction; chi tiết tuân theo abstraction. |

Nguồn khái niệm: [Robert C. Martin — SOLID Relevance](https://blog.cleancoder.com/uncle-bob/2020/10/18/Solid-Relevance.html). Các ví dụ dưới đây được diễn giải từ code AI@HR.

### 12.2. S — Single Responsibility Principle

Trong import weight, có những loại thay đổi khác nhau: đổi điều kiện file đầu vào, đổi quy tắc nội dung Markdown, đổi luồng use case hoặc đổi cách lưu database. Phân chia hiện tại giúp tìm được phần cần sửa cho từng nhu cầu:

| Thành phần | Trách nhiệm cụ thể |
|---|---|
| `ImportWeightCommandValidator` | Kiểm tra tên, file, kích thước, phần mở rộng. |
| `WeightValidator` | Kiểm tra nội dung Markdown và trọng số. |
| `ImportWeightCommandHandler` | Điều phối các bước import và trả kết quả. |
| `WeightRepository` | Truy cập và lưu dữ liệu. |

Một class có nhiều method vẫn có thể phục vụ một trách nhiệm thống nhất. Ngược lại, chỉ tách nhiều class chưa chứng minh thiết kế tốt; cần giải thích chúng thay đổi vì lý do gì. Xem thêm về SRP và separation of concerns: [Microsoft — Architectural principles](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles).

**Cách nói:**

> Em áp dụng việc phân chia trách nhiệm khi tách kiểm tra input, kiểm tra nội dung weight, điều phối import và persistence. Ví dụ đổi quy tắc tổng trọng số thì em xem WeightValidator; đổi cách query dữ liệu thì xem repository. Em hiểu SRP theo lý do thay đổi, không theo số method.

### 12.3. O — Open/Closed Principle

Ví dụ trong dự án: [ValidationBehavior](backend/dotnet/AI@HR/src/Shared/AI.HR.Shared.Application/Common/Behaviors/ValidationBehavior.cs) nhận `IEnumerable<IValidator<TMessage>>`, chạy các validator rồi tổng hợp lỗi. Các rule cụ thể ở từng validator.

Dự án có đăng ký scan validators trong assembly. Khi thêm validator cho command trong assembly đã đăng ký, cơ chế tổng hợp lỗi không cần biết nội dung rule mới. Nếu thêm assembly khác thì vẫn cần cấu hình phù hợp.

**Cách nói:**

> Em có thể dùng ValidationBehavior để giải thích OCP. Phần behavior xử lý việc gọi validator và gom lỗi chung; rule nằm ở các validator. Khi thêm validator cho command trong assembly đã đăng ký, không cần sửa thuật toán chung trong behavior.

Việc thay đổi một quy tắc đang có vẫn có thể cần sửa validator. Không cần tạo cơ chế mở rộng cho mọi khả năng chưa có nhu cầu. Đây là ví dụ kiến trúc dùng chung trong dự án, chưa xác nhận behavior là phần bạn trực tiếp viết.

### 12.4. L — Liskov Substitution Principle

Hợp đồng gồm ý nghĩa input, output, lỗi và tác động mà phía gọi dựa vào. Việc cùng implement một interface chưa đủ để bảo đảm chúng thay thế nhau đúng.

Ví dụ minh họa bằng `IWeightRepository`: nếu handler dựa vào việc `AddAsync` hoàn thành nghĩa là thao tác thêm dữ liệu đã được thực hiện theo contract, một implementation âm thầm bỏ qua việc lưu sẽ làm phía gọi hiểu sai. Đây là tình huống giả định để giải thích nguyên tắc, không phải lỗi đã tìm thấy trong dự án.

**Cách nói:**

> Em hiểu LSP là khi thay implementation thì phía sử dụng vẫn nhận được hành vi đúng theo contract. Với repository, em cần quan tâm cả kết quả, ý nghĩa lỗi và tác động lưu dữ liệu. Trong dự án em có làm việc với abstraction và mock, nhưng việc có mock chưa chứng minh mọi implementation tuân thủ LSP.

Chưa tìm thấy một refactor LSP riêng để nhận là thành tích cá nhân. Bạn nên hiểu lý thuyết và dùng ví dụ minh họa này nếu được hỏi.

### 12.5. I — Interface Segregation Principle

Trong code hiện tại, [IWeightValidator](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Application/Weights/Validators/IWeightValidator.cs) có hành vi kiểm tra nội dung weight; [IJdCommandPublisher](backend/dotnet/AI@HR/src/Modules/Jobs/AI.HR.Jobs.Application/Common/Interfaces/IJdCommandPublisher.cs) có hành vi gửi lệnh parse JD.

Phía import weight cần khả năng kiểm tra nội dung; phía gửi JD cần khả năng gửi command. Có thể giải thích bằng hai nhu cầu riêng này mà không giả định một service tổng hợp mọi việc.

**Cách nói:**

> Em hiểu ISP là để client phụ thuộc interface phù hợp với nhu cầu của nó. Trong dự án có các interface tập trung như IWeightValidator và IJdCommandPublisher. Em dùng chúng làm ví dụ về phạm vi interface rõ ràng; em chưa có một refactor riêng từ interface lớn để minh họa sâu hơn.

Không có quy tắc mọi interface phải có đúng một method. Điều cần đánh giá là các client có bị phụ thuộc những hành vi không liên quan hay không.

### 12.6. D — Dependency Inversion Principle

Trong PR #291, `dea1a561`, bạn có thay đổi trực tiếp để Application định nghĩa nhu cầu gửi command qua interface; Infrastructure giữ cách hiện thực hóa việc gửi message. Hướng reference của source code đi theo abstraction, trong khi lúc chạy handler vẫn có thể gọi implementation ở Infrastructure. Tham khảo nguyên tắc dependency inversion: [Microsoft — Architectural principles](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles).

Luồng để nhớ:

```text
Application:
  Use case sử dụng IJdCommandPublisher
  IJdCommandPublisher mô tả hành vi cần dùng

Infrastructure:
  JdCommandPublisher implements IJdCommandPublisher
  Giữ chi tiết gửi message, topology/routing

Composition root / DI:
  Kết nối interface với implementation
```

**DIP và DI:** DIP là nguyên tắc tổ chức dependency. DI là cách cung cấp dependency từ bên ngoài, chẳng hạn qua constructor và container. Việc inject một class cụ thể chưa tự chứng minh đã áp dụng đúng DIP.

**Cách nói:**

> Trong import JD, em đã tách Application khỏi Shared.Infrastructure, định nghĩa IJdCommandPublisher trong Application và đưa chi tiết gửi message sang Infrastructure. Use case chỉ cần diễn đạt nhu cầu gửi lệnh parse JD. Đây là ví dụ em áp dụng DIP; DI cung cấp implementation khi ứng dụng chạy.

### 12.7. Câu trả lời tổng hợp về SOLID

> Em hiểu SOLID là năm nguyên tắc giúp tổ chức trách nhiệm và dependency để code dễ thay đổi và kiểm tra hơn. Trong internship, ví dụ em giải thích rõ nhất là phân chia validation, use case và persistence, cùng việc tách messaging qua IJdCommandPublisher. Em có thay đổi bỏ reference từ Application sang Shared.Infrastructure trong luồng import JD. Em cũng hiểu cần áp dụng theo nhu cầu thực tế và tránh thêm abstraction chỉ để đủ pattern.

<a id="13-clean-code"></a>

## 13. Clean Code — code rõ nghĩa, thuận tiện cho người cùng làm

### 13.1. Khái niệm

Clean Code nói đến cách viết và tổ chức code để người đọc hiểu đúng ý định, tìm được hành vi liên quan và thay đổi thuận lợi. Những thực hành có ích gồm tên thể hiện mục đích, phong cách nhất quán, logic dễ theo dõi và xử lý lỗi phù hợp. Coding conventions hỗ trợ khả năng đọc và cộng tác. Tham khảo: [Microsoft — C# coding conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).

Đây không phải một phép chấm điểm tuyệt đối bằng số dòng hoặc số comment. Khi giải thích, hãy chỉ vào một thay đổi giúp code dễ hiểu hay ít phải sửa ở nhiều nơi hơn.

### 13.2. Các thực hành và ví dụ của bạn

| Thực hành | Cách áp dụng trong AI@HR |
|---|---|
| Tên có ý nghĩa | `535f4e27` đổi mapper thành `ProcessedJdResultMapper`, diễn đạt dữ liệu được map. |
| Trách nhiệm rõ | Validator kiểm tra rule, handler điều phối, repository xử lý persistence. |
| DRY | `f0d4c5c2`: hai handler file/Markdown dùng chung `JobSubmissionExecutor` cho luồng submit. |
| Quy tắc chuẩn hóa nhất quán | `3f132294`: dùng `Weight.NormalizeContent` cho nội dung weight. |
| Thu gọn thứ chưa cần | `a74a223a`: bỏ phương thức/tham số ở UserRepository và chỉnh caller/test. |
| Kiểm tra hành vi sau sửa | Các test đi cùng phần sửa weight validation và xử lý lỗi import. |

### 13.3. DRY, KISS và YAGNI ở mức dễ giải thích

- **DRY — Don't Repeat Yourself:** Tránh định nghĩa cùng một quy tắc ở nhiều nơi phải cập nhật đồng thời. Hai đoạn giống cú pháp chưa chắc cùng một quy tắc. Ví dụ submit JD từ file và text có các bước chung thật sự nên dùng chung executor. Xem thêm [Microsoft — DRY](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles#dont-repeat-yourself-dry).
- **KISS — Keep It Simple:** Ưu tiên giải pháp dễ hiểu đáp ứng yêu cầu. Câu hỏi áp dụng vào code của mình: người khác có theo dõi được luồng đọc input → kiểm tra → gửi command không? Có thêm tầng nào mà mình không giải thích được mục đích không?
- **YAGNI — You Aren't Gonna Need It:** Cân nhắc nhu cầu hiện tại trước khi thêm khả năng dự phòng. Khi ôn PR #328, xem lý do phạm vi UserRepository được thu gọn; Git chưa xác nhận đây là động cơ người review gọi tên YAGNI.

KISS/YAGNI ở đây là khung tự giải thích lựa chọn thiết kế, không phải nhãn gán sẵn cho mỗi commit. Các nguyên tắc cần được cân nhắc cùng yêu cầu mở rộng thực tế.

### 13.4. Refactor là gì?

Refactor là điều chỉnh cấu trúc bên trong, hướng đến giữ nguyên hành vi quan sát được. Ví dụ để luyện là gom phần submit chung hoặc đổi tên cho rõ nghĩa. Cần xem contract và test để kiểm tra việc thay đổi có giữ hành vi hay không.

Phân biệt hai loại thay đổi trong lịch sử của bạn:

- Gom luồng submit dùng chung chủ yếu là điều chỉnh cấu trúc.
- Sửa cách hiểu `1%` là thay đổi hành vi để đáp ứng quy tắc mong muốn.

Một PR có thể chứa cả hai loại. Việc có test hỗ trợ kiểm tra nhưng không tự bảo đảm mọi hành vi được giữ nguyên.

### 13.5. Clean Code, SOLID và Clean Architecture liên quan thế nào?

| Nội dung | Trọng tâm | Ví dụ để nhớ |
|---|---|---|
| Clean Code | Cách viết code rõ nghĩa và dễ thay đổi. | Tên mapper, luồng submit, tránh lặp. |
| SOLID | Trách nhiệm, hợp đồng và dependency giữa thành phần. | Validator riêng, abstraction gửi command. |
| Clean Architecture | Boundary và chiều phụ thuộc ở cấp tổ chức ứng dụng. | Application định nghĩa interface, Infrastructure implement. |

Việc có folder Domain/Application/Infrastructure chưa đủ chứng minh boundary đúng. Cần nhìn reference và nơi chứa logic; commit DIP của bạn là một ví dụ cụ thể để giải thích điều này.

### 13.6. Câu trả lời mẫu

> Em hiểu clean code là code giúp người khác đọc hiểu và sửa thuận lợi. Trong import JD, hai đầu vào file và Markdown có phần submit chung, nên em gom các bước đó vào JobSubmissionExecutor, còn phần đọc input giữ ở handler tương ứng. Qua review, em cũng thu gọn UserRepository và cập nhật các test liên quan. Em học được rằng cần tránh lặp những phần có cùng ý nghĩa và giữ trách nhiệm rõ; việc thêm class hay rút ngắn code phải phục vụ mục đích đó.

<a id="14-agile-va-scrum"></a>

## 14. Agile process và Scrum

### 14.1. Agile là gì?

Agile là cách tiếp cận phát triển theo phản hồi và thích ứng, với các giá trị nêu trong Agile Manifesto:

| Giá trị được ưu tiên | Cách hiểu |
|---|---|
| Con người và tương tác | Chú trọng việc cùng hiểu và phối hợp. |
| Phần mềm hoạt động | Đánh giá tiến độ bằng kết quả sử dụng được. |
| Hợp tác với khách hàng | Cùng làm rõ và điều chỉnh nhu cầu. |
| Thích ứng với thay đổi | Cập nhật cách làm khi có thông tin mới. |

Quy trình/công cụ, tài liệu, hợp đồng và kế hoạch vẫn có giá trị. Nguồn: [Agile Manifesto](https://agilemanifesto.org/).

Những ý nên hiểu thêm: giao giá trị theo các phần nhỏ; phát triển với nhịp bền vững; chú ý chất lượng kỹ thuật; trao đổi thường xuyên; nhìn lại cách làm để cải thiện. Nguồn: [Principles behind the Agile Manifesto](https://agilemanifesto.org/principles.html).

Ví dụ để tư duy từ dự án: một endpoint import trả response thành công chưa đủ để kết luận nhu cầu đã được đáp ứng. Cần xem dữ liệu được hiểu đúng và kết quả phù hợp với hành vi người dùng mong đợi. Với phần weight, các rule về đơn vị và tổng trọng số giúp cụ thể hóa điều đó.

### 14.2. Iterative và incremental

Hai từ này giúp bạn giải thích cách làm qua các phiên bản:

- **Iterative:** Quay lại điều chỉnh một phần đã làm sau khi có thêm thông tin. Trong lịch sử của bạn có các lần sửa validator và repository.
- **Incremental:** Bổ sung các phần chức năng. Trong dự án có các phần như import JD, quản lý job, weight configuration và nhận kết quả xử lý.

Một cách làm có thể kết hợp cả hai. Chỉ có nhiều commit hoặc nhiều branch chưa đủ để kết luận team thực hiện đầy đủ Agile; cần xét giá trị và phản hồi trong cách làm thực tế.

### 14.3. Scrum là gì?

Scrum là một framework. Các khái niệm cơ bản dưới đây theo [Scrum Guide](https://scrumguides.org/scrum-guide.html):

- **Ba trụ cột:** minh bạch, kiểm tra và thích ứng.
- **Trách nhiệm trong Scrum Team:** Product Owner tối đa hóa giá trị và quản lý Product Backlog; Scrum Master giúp thiết lập Scrum và nâng hiệu quả team; Developers tạo Increment đáp ứng Definition of Done.
- **Sprint:** Chu kỳ một tháng hoặc ngắn hơn, bao chứa các sự kiện khác.
- **Sprint Planning:** Thống nhất mục tiêu và kế hoạch công việc.
- **Artifacts và commitments:** Product Backlog — Product Goal; Sprint Backlog — Sprint Goal; Increment — Definition of Done.

Đây là lý thuyết để hiểu framework. Chưa xác nhận team internship của bạn thực hiện đầy đủ các thành phần này.

### 14.4. Ba sự kiện dễ bị hỏi nhầm

| Sự kiện | Mục đích | Cách phân biệt |
|---|---|---|
| Daily Scrum | Developers kiểm tra tiến độ hướng đến Sprint Goal và điều chỉnh kế hoạch. | Cuộc trao đổi ngắn hướng đến phối hợp công việc; không chỉ báo cáo cho quản lý. |
| Sprint Review | Team và stakeholder xem kết quả, thay đổi bối cảnh và hướng tiếp theo. | Trao đổi về sản phẩm/giá trị; không đồng nghĩa code review của PR. |
| Sprint Retrospective | Nhìn lại cách làm để cải thiện chất lượng và hiệu quả. | Trao đổi về quá trình làm việc và hành động cải thiện. |

Nguồn cho từng sự kiện: [Daily Scrum](https://www.scrum.org/resources/what-is-a-daily-scrum), [Sprint Review](https://www.scrum.org/resources/what-is-a-sprint-review), [Sprint Retrospective](https://www.scrum.org/resources/what-is-a-sprint-retrospective).

Ví dụ gợi ý để phân biệt: xem kết quả import có đáp ứng nhu cầu là câu hỏi về sản phẩm; bàn cách làm rõ requirement sớm hơn là câu hỏi về cách làm. Đây là minh họa khái niệm, không phải biên bản một cuộc họp YPP.

### 14.5. Acceptance criteria và Definition of Done

**AC** mô tả điều kiện của một yêu cầu cụ thể. Với phần weight, code/test hiện có thể hiện quy tắc tổng trọng số và định dạng hợp lệ.

**Definition of Done** mô tả trạng thái chất lượng cần đạt của Increment. Một bộ tiêu chí DoD minh họa có thể bao gồm review, kiểm tra hành vi và điều kiện tích hợp; đây không phải DoD đã được xác nhận của team bạn. Khái niệm DoD theo [Scrum Guide](https://scrumguides.org/scrum-guide.html).

Điều cần giải thích khi phỏng vấn: một feature có thể đáp ứng một AC nhưng vẫn chưa đủ điều kiện chất lượng chung để gọi là Done. Git hiện giúp xem code/test, chưa cho biết văn bản DoD hoặc toàn bộ quyết định nghiệm thu của team.

### 14.6. Bạn đã ứng dụng Agile thế nào?

Bằng chứng mạnh nhất là vòng phát triển và phản hồi:

```text
Yêu cầu/AC
    ↓
Implementation và test
    ↓
Review
    ↓
Điều chỉnh code và test
```

Đây là sơ đồ để giải thích các hoạt động đã tìm thấy, không phải khẳng định mọi PR có cùng một quy trình cứng. PR #319 có commit sửa theo AC; PR #291 và #328 có commit ghi xử lý review comments/feedback.

### 14.7. Câu trả lời mẫu

> Em hiểu Agile là cách làm có vòng phản hồi để team thích ứng và tạo giá trị theo từng phần. Trong internship, em làm các phần như import JD và manage weight qua những PR riêng, bổ sung test và điều chỉnh sau review. Ví dụ ở PR endpoints/tests, em sửa UserRepository và các test liên quan sau feedback. Ở phần weight, em có thay đổi sửa validator theo AC. Em học được rằng cần tiếp tục kiểm tra cách hiểu yêu cầu và cải thiện implementation trong quá trình phát triển.

Nếu nhớ các hoạt động trao đổi PO hoặc cập nhật tiến độ, thêm đúng trải nghiệm đó. Lịch họp, thời lượng Sprint và người giữ trách nhiệm Scrum Master chưa được xác nhận từ dữ liệu Git đã đọc.

<a id="15-ban-on-nhanh"></a>

## 15. Bản ôn nhanh trước phỏng vấn

| Chủ đề | Một câu khái niệm | Ví dụ nên ưu tiên |
|---|---|---|
| SDLC | Vòng đời từ nhu cầu đến phát triển, triển khai và duy trì. | Weight: AC → validation → test → review. |
| SOLID | Nguyên tắc tổ chức trách nhiệm, hợp đồng và dependency. | Tách Application khỏi chi tiết messaging qua IJdCommandPublisher. |
| Clean Code | Viết và tổ chức code để người khác hiểu, sửa thuận lợi. | Hai đầu vào JD dùng chung phần submit; thu gọn UserRepository. |
| Agile | Phát triển theo phản hồi và thích ứng để tạo giá trị. | Chỉnh implementation và test sau PR feedback hoặc xem lại AC. |
| Scrum | Framework với trách nhiệm, sự kiện và artifacts cụ thể. | Hiểu lý thuyết; chỉ kể các hoạt động YPP mình thực sự tham gia. |

Các câu hỏi tự kiểm tra:

1. Trong SDLC, em đã trực tiếp tham gia phần nào và còn muốn học phần nào?
2. AC ở weight configuration được chuyển thành rule/test nào?
3. Vì sao `1%` khác `1`, và test hiện tại kiểm tra điều gì?
4. S của SOLID có phải mỗi class chỉ một method không? Vì sao?
5. Vì sao IJdCommandPublisher đặt ở Application? Implementation nằm ở đâu?
6. DIP khác DI thế nào?
7. Vì sao gom logic submit chung? Có phần nào nên giữ riêng?
8. Refactor khác sửa hành vi lỗi thế nào?
9. Daily Scrum, Sprint Review và Retrospective có mục đích gì?
10. Vòng feedback nào của mình có thể chỉ ra bằng PR/commit?

Mục tiêu luyện: giải thích khái niệm bằng lời của mình, kể một ví dụ rồi trả lời tiếp được “vì sao”. Các ví dụ code và PR giúp bạn nhớ lại công việc; không cần học thuộc toàn bộ hash hoặc lời văn trong tài liệu.
