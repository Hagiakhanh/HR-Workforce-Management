# Script thuyết trình YPP Program-End Demo

**Người trình bày:** Hà Gia Khánh  
**Dự án:** AI@HR  
**Thời lượng mục tiêu:** 15–17 phút; tối đa 20 phút nếu có tương tác ngắn

**Phân bổ thời gian gợi ý:**

- Slide 1–2 — Mở đầu và agenda: 1 phút 15 giây
- Slide 3–5 — Kiến trúc dự án: 3 phút 30 giây
- Slide 6–8 — Đóng góp và kiến thức/công nghệ: 4 phút
- Slide 9 — Lộ trình lên Middle level: 1 phút 30 giây
- Slide 10–12 — Ba câu chuyện STAR: 4 phút 30 giây
- Slide 13–14 — Tổng kết và kết thúc: 1 phút 15 giây

**Tổng mục tiêu:** khoảng 16 phút

---

## Slide 1 — My YPP Journey — AI@HR

Xin chào mọi người. Em là Hà Gia Khánh.

Hôm nay em xin chia sẻ về hành trình của mình trong chương trình YPP thông qua dự án AI@HR. Phần trình bày sẽ không đi sâu vào việc demo toàn bộ sản phẩm, mà tập trung vào góc nhìn cá nhân của em: em hiểu kiến trúc dự án như thế nào, em đã trực tiếp thực hiện những phần nào, những kiến thức em học được, và các thách thức em đã gặp trong quá trình phát triển.

Qua dự án này, điều em học được không chỉ là cách sử dụng một framework cụ thể, mà còn là cách kết nối nhiều thành phần để tạo thành một hệ thống hoàn chỉnh và có khả năng xử lý lỗi.

**Chuyển ý:** Trước tiên, em xin giới thiệu nhanh cấu trúc của phần trình bày.

---

## Slide 2 — Agenda

Phần trình bày của em gồm năm nội dung chính.

Đầu tiên là kiến trúc tổng thể của dự án và kiến trúc bên trong .NET backend.

Tiếp theo, em sẽ trình bày những phần mà em đã trực tiếp phát triển hoặc cải thiện.

Sau đó là những kiến thức kỹ thuật em học được trong dự án, cũng như một số chủ đề em tự học thêm như Design Patterns, Clean Architecture, API design và ASP.NET Core.

Cuối cùng, em sẽ chia sẻ những nội dung muốn tìm hiểu sâu hơn và một số thách thức tiêu biểu cùng cách em giải quyết.

**Chuyển ý:** Trước khi nói về phần việc cá nhân, em xin bắt đầu với bức tranh tổng thể của hệ thống.

---

## Slide 3 — AI@HR Architecture at a Glance

Ở mức tổng quan, AI@HR là một hệ thống polyglot, tức là sử dụng nhiều nền tảng và ngôn ngữ khác nhau.

Người dùng tương tác với React Web App. Frontend gửi các request HTTP đến .NET Web API và nhận những cập nhật theo thời gian thực thông qua SignalR.

.NET Web API chịu trách nhiệm cho business API, authentication và dữ liệu ứng dụng trong PostgreSQL. Những công việc AI có thời gian xử lý dài sẽ không được thực hiện trực tiếp trong HTTP request. Thay vào đó, Web API gửi command qua RabbitMQ để Java AI Worker xử lý.

Java Worker chịu trách nhiệm đọc CV hoặc Job Description, gọi Cloud LLM và trả kết quả về thông qua message broker. File Storage được sử dụng để lưu các tài liệu CV và JD.

Điểm quan trọng của kiến trúc này là việc gọi AI có thể mất nhiều giây hoặc gặp lỗi tạm thời. RabbitMQ giúp tách quá trình đó khỏi request của người dùng. Nếu AI Worker hoặc LLM gặp sự cố, hệ thống có thể giảm tốc độ xử lý nhưng Web API và giao diện vẫn hoạt động.

Ngoài ra, do backend sử dụng cả C# và Java, các message contract được định nghĩa bằng AsyncAPI và JSON Schema. Từ một nguồn định nghĩa chung, dự án có thể sinh ra contract cho cả hai ngôn ngữ, hạn chế việc hai bên hiểu khác nhau về cùng một message.

**Chuyển ý:** Ở slide tiếp theo, em sẽ nhìn cụ thể hơn vào trách nhiệm của từng container.

---

## Slide 4 — Container Architecture

Trong kiến trúc container, mỗi thành phần có một phạm vi trách nhiệm khá rõ ràng.

React Web App chỉ giao tiếp với .NET Web API. Frontend không truy cập trực tiếp RabbitMQ, database hoặc file system.

.NET Web API quản lý business API và toàn bộ dữ liệu trong PostgreSQL. Khi có một tác vụ cần AI xử lý, API publish command vào RabbitMQ. Java AI Worker nhận command, đọc file cần thiết, gọi Cloud LLM và publish kết quả trở lại.

RabbitMQ sử dụng asynchronous messaging và quorum queue để tăng độ tin cậy. Việc giao tiếp bất đồng bộ cũng cho phép Web API và AI Worker được scale tương đối độc lập.

Một thành phần khác là Message Contracts. Thành phần này không chạy như một service, nhưng đóng vai trò là nguồn sự thật chung cho cấu trúc message và topology của RabbitMQ.

Từ kiến trúc này, em hiểu rõ hơn ba lợi ích của messaging: giữ giao diện phản hồi nhanh, xử lý lỗi có kiểm soát, và giảm coupling giữa các service.

**Chuyển ý:** Sau bức tranh toàn hệ thống, em xin đi sâu vào phần .NET backend, cũng là khu vực em tham gia nhiều nhất.

---

## Slide 5 — .NET Modular Monolith

.NET backend được xây dựng theo hướng Modular Monolith. Nghĩa là các module vẫn được đóng gói trong một Web API deployable, nhưng mỗi module có domain, application logic và persistence boundary riêng.

Identity Module quản lý user, role và authentication bằng OpenIddict. Jobs Module quản lý Job Description, tiêu chí tuyển dụng, prompt, weight và quá trình xử lý JD. Screening Module quản lý batch CV và kết quả screening.

API and Realtime là điểm giao tiếp với frontend, sử dụng FastEndpoints, JWT và SignalR. Dữ liệu được lưu trong PostgreSQL với schema riêng cho từng module. Những luồng bất đồng bộ giao tiếp với RabbitMQ thông qua MassTransit.

Hai khu vực em tập trung nhiều nhất là Identity Module và Jobs Module. Khi thực hiện các phần này, em phải suy nghĩ không chỉ về endpoint, mà còn về domain entity, application use case, repository, database configuration, message consumer và test.

Điều em rút ra là Modular Monolith không chỉ là việc chia project thành nhiều folder. Boundary chỉ thực sự có ý nghĩa khi dependency và quyền sở hữu dữ liệu được kiểm soát rõ ràng.

**Chuyển ý:** Từ kiến trúc đó, đây là những phần việc cụ thể mà em đã đóng góp.

---

## Slide 6 — My Contribution to AI@HR

Đóng góp đầu tiên của em là phần nền tảng cho .NET backend, bao gồm .NET Aspire, cấu trúc Modular Monolith và việc áp dụng Clean Architecture vào các module.

Ở phần Identity và Security, em triển khai authentication với OpenIddict, JWT validation, user repository và các API như register, token và logout. Sau đó em tiếp tục refactor module theo hướng CQS và repository pattern, đồng thời bổ sung unit test.

Ở Jobs và Weights, em tham gia các chức năng import và quản lý Job Description, xử lý criteria, xây dựng rich domain model, và các chức năng import, update, delete weight. Em cũng thực hiện các endpoint liên quan đến screening và kết nối một số flow với frontend.

Ở phần integration và quality, em làm việc với EF Core, PostgreSQL, RabbitMQ, MassTransit consumer, SignalR notification, validation, unit test và integration test.

Quy trình làm việc thường đi qua bốn bước: đầu tiên là model domain; tiếp theo là command, query và API; sau đó tích hợp persistence, messaging hoặc UI; cuối cùng là validation, retry và test.

Điều quan trọng nhất em học được là một feature không kết thúc khi endpoint đã chạy. Nó chỉ thực sự hoàn chỉnh khi domain, database, API contract, integration flow và test hoạt động nhất quán với nhau.

**Chuyển ý:** Từ các phần việc này, em tổng hợp được bốn nhóm kiến thức kỹ thuật chính.

---

## Slide 7 — My Foundation Before AI@HR

Trước khi tham gia dự án AI@HR, em đã tìm hiểu một số kiến thức nền tảng về phát triển backend và thiết kế phần mềm.

Đầu tiên là Clean Code. Em học cách đặt tên có ý nghĩa, chia method theo một trách nhiệm rõ ràng và ưu tiên code dễ đọc, dễ bảo trì thay vì chỉ làm cho chức năng chạy được.

Tiếp theo là các nguyên tắc SOLID, đặc biệt là Single Responsibility và Dependency Inversion. Những nguyên tắc này giúp em hiểu vì sao một class không nên đảm nhiệm quá nhiều vai trò và vì sao business logic nên phụ thuộc vào abstraction thay vì implementation cụ thể.

Em cũng đã tìm hiểu một số Design Pattern như Repository, Factory, Strategy và Mediator. Ở giai đoạn này, em hiểu mục đích cơ bản của từng pattern, nhưng chưa có nhiều cơ hội quan sát trade-off của chúng trong một hệ thống lớn.

Với Clean Architecture, em đã biết về dependency direction, separation of concerns, và ý tưởng tách domain khỏi infrastructure.

Về công nghệ, em có nền tảng ASP.NET Core như Dependency Injection, middleware, authentication và Entity Framework Core cơ bản. Em cũng học các nguyên tắc thiết kế REST API như resource, HTTP method, status code, validation và error response.

Tuy nhiên, trước dự án các kiến thức này vẫn khá tách rời. AI@HR là cơ hội để em kết nối chúng trong một hệ thống thực tế.

**Chuyển ý:** Bên cạnh những công nghệ xuất hiện trực tiếp trong dự án, em cũng dành thời gian học thêm các kiến thức nền tảng.

---

## Slide 8 — Technologies Applied in AI@HR

Khi bước vào AI@HR, em bắt đầu sử dụng các nền tảng, architectural model và framework cụ thể để áp dụng những kiến thức ở slide trước.

Công nghệ chính là .NET 10 cùng ASP.NET Core. Em dùng nền tảng này để xây dựng backend, cấu hình Dependency Injection và middleware. Về mô hình kiến trúc, backend được tổ chức theo Modular Monolith: các module có boundary, domain và persistence riêng nhưng vẫn được triển khai trong một Web API thống nhất.

Đối với API, em làm việc với FastEndpoints để xây dựng endpoint và Scalar để cung cấp tài liệu cũng như giao diện kiểm thử API. Bên trong từng module, em áp dụng CQRS để tách command và query, đồng thời dùng Mediator để điều phối request đến handler tương ứng. Như vậy, Modular Monolith xác định boundary ở cấp hệ thống, còn CQRS và Mediator giúp tổ chức application flow bên trong module.

Ở tầng dữ liệu, em sử dụng Entity Framework Core với PostgreSQL. Qua đó em làm việc với repository, LINQ query, tracking và no-tracking, migration, entity configuration và schema riêng cho từng module.

Ở phần xác thực, em sử dụng OpenIddict và JWT cho các flow đăng ký, lấy token, logout và kiểm tra access token.

Đối với giao tiếp bất đồng bộ, em sử dụng RabbitMQ và MassTransit để cấu hình command, consumer, retry và error queue. SignalR được dùng để cập nhật tiến trình xử lý về frontend theo thời gian thực.

Em cũng làm việc với .NET Aspire và Docker để orchestration môi trường local, quản lý dependency và thứ tự khởi động giữa PostgreSQL, RabbitMQ, topology initializer và Web API.

Các công nghệ hỗ trợ khác gồm FluentValidation, Markdig, xUnit, Moq, FluentAssertions và Testcontainers.

Điểm khác biệt lớn nhất là em không còn nhìn từng công nghệ như một phần riêng lẻ. Em hiểu rõ hơn cách chúng phối hợp trong một flow end-to-end, từ request, validation, business logic, database, messaging cho đến realtime update và testing.

**Chuyển ý:** Tuy nhiên, vẫn còn nhiều chủ đề mà em mới chỉ chạm đến và muốn tiếp tục đào sâu.

---

## Slide 9 — My Growth Path Toward Middle Level

Mục tiêu tiếp theo của em không chỉ là học thêm nhiều công nghệ, mà là phát triển từ nền tảng hiện tại để có thể đảm nhiệm vai trò Middle Backend Developer.

Năng lực đầu tiên em muốn phát triển là khả năng đưa ra quyết định kiến trúc. Hiện tại em đã áp dụng Modular Monolith, Clean Architecture, CQRS và Mediator. Bước tiếp theo là hiểu khi nào nên hoặc không nên dùng những cách tiếp cận này, tự xác định boundary và giải thích được trade-off của quyết định kỹ thuật.

Năng lực thứ hai là xây dựng backend sẵn sàng cho production. Từ kinh nghiệm với EF Core, PostgreSQL, RabbitMQ và MassTransit, em muốn đào sâu về concurrency, idempotency, transaction, resilience, performance và security. Mục tiêu là không chỉ triển khai happy path mà còn dự đoán và xử lý được failure path.

Năng lực thứ ba là observability và delivery. Hiện tại em đã làm việc với Aspire, Docker, unit test và integration test. Để tiến lên Middle level, em muốn có khả năng sử dụng tracing, metric và log để điều tra sự cố, thực hiện profiling và load test, đồng thời hiểu CI/CD, safe database migration, rollout và rollback.

Năng lực cuối cùng là ownership và ảnh hưởng trong team. Thay vì chỉ nhận một task đã được mô tả sẵn, em muốn có khả năng làm rõ requirement, chia nhỏ và estimate công việc, đề xuất technical design, trao đổi trade-off với đồng đội, review code và theo dõi feature sau khi triển khai.

Kết quả em hướng đến là có thể tự sở hữu một feature có độ phức tạp trung bình từ lúc nhận requirement, thiết kế, triển khai, kiểm thử cho đến hỗ trợ khi chạy trong môi trường thực tế.

**Chuyển ý:** Những mục tiêu này xuất phát trực tiếp từ các thách thức em đã gặp trong dự án.

---

## Slide 10 — Challenge 1: Working Across .NET and Java

Thách thức đầu tiên em trình bày theo mô hình STAR là làm việc trong một dự án sử dụng nhiều công nghệ, cụ thể là .NET Web API và Java AI Worker.

**Situation:** Hai backend sử dụng ngôn ngữ, framework, build tool và data model khác nhau, nhưng phải cùng xử lý một flow Job Description thông qua RabbitMQ. Một thay đổi nhỏ về tên field, serialization hoặc message version ở một bên có thể làm bên còn lại xử lý sai. Khi xảy ra lỗi, em cũng phải xác định vấn đề nằm ở Web API, broker, contract hay Java Worker.

**Task:** Nhiệm vụ của em là hiểu flow end-to-end, giúp hai stack sử dụng cùng một contract, và làm cho lỗi có thể được theo dõi xuyên qua boundary giữa các service.

**Action:** Em đọc và tìm hiểu flow xử lý ở phía Java thay vì chỉ nhìn phần .NET. Em sử dụng AsyncAPI và JSON Schema làm định nghĩa chung, đồng bộ naming và serialization, cấu hình MassTransit consumer, correlation, retry và error handling. Em cũng bổ sung integration test để kiểm tra flow qua nhiều thành phần.

**Result:** Hai backend có một nguồn contract chung, các lỗi tích hợp dễ xác định hơn, và test giúp hạn chế contract drift khi một bên thay đổi. Quan trọng hơn, em học được cách tiếp cận một hệ thống không chỉ từ công nghệ quen thuộc của mình mà từ toàn bộ luồng nghiệp vụ.

**Chuyển ý:** Thách thức thứ hai không nằm ở kết nối giữa các service, mà nằm ở cách giữ code bên trong backend dễ phát triển khi số lượng feature tăng.

---

## Slide 11 — Challenge 2: Keeping Module Boundaries Clear

**Situation:** Khi Identity và Jobs Module phát triển, một feature có thể đồng thời chạm đến API, business rule, database và messaging. Nếu không kiểm soát, logic dễ bị đặt sai layer, repository bị sử dụng trực tiếp và code giữa các module bắt đầu phụ thuộc lẫn nhau.

**Task:** Em cần refactor cấu trúc nhưng vẫn giữ nguyên behavior hiện tại, đồng thời làm cho các use case mới dễ tìm, dễ test và an toàn hơn khi mở rộng.

**Action:** Em tách rõ Domain, Application và Infrastructure; sử dụng CQRS và Mediator để tổ chức command, query và handler; áp dụng repository và schema ownership; cải thiện encapsulation của entity; đồng thời bổ sung validation và automated test sau các vòng code review.

**Result:** Boundary của Identity và Jobs trở nên rõ ràng hơn. Use case dễ xác định và test hơn, còn thay đổi ở persistence hoặc infrastructure ít có khả năng lan sang domain và module khác.

Bài học em rút ra là boundary không chỉ nằm trong sơ đồ kiến trúc. Nó cần được thể hiện và kiểm soát trong dependency, code structure, data ownership và test.

**Chuyển ý:** Từ hai thách thức này và toàn bộ quá trình làm dự án, em có ba kết luận chính.

---

## Slide 12 — Challenge 3: Turning Requirements into Teamwork

Thách thức thứ ba thiên về con người: cách em tiếp nhận requirement, trao đổi trực tiếp với PO và phối hợp với team để hoàn thành task.

**Situation:** Một số task ban đầu chỉ có requirement ở mức tổng quan. Trong quá trình thực hiện, khi trao đổi và nhận feedback trực tiếp từ PO, em phát hiện thêm business case, edge case hoặc sự thay đổi về mức độ ưu tiên. Đồng thời, thay đổi của em có thể ảnh hưởng đến phần việc của các thành viên khác trong team.

**Task:** Em cần hiểu đúng vấn đề PO muốn giải quyết, chuyển feedback thành phạm vi và acceptance criteria cụ thể, sau đó phối hợp với team để thống nhất cách làm và hoàn thành task đúng mục tiêu.

**Action:** Em trao đổi trực tiếp với PO, diễn đạt lại requirement theo cách hiểu của mình để xác nhận expected outcome, đồng thời hỏi thêm các ví dụ và edge case. Sau đó em chia task thành các bước rõ ràng, thông báo dependency và tiến độ cho team, chủ động nhờ feedback khi còn điểm chưa chắc chắn, rồi cùng các thành viên xử lý blocker và điều chỉnh implementation.

**Result:** Task được hoàn thành với ít rework hơn vì kỳ vọng của PO, phạm vi công việc và trách nhiệm của từng người rõ ràng hơn. Quan trọng hơn, em cải thiện được cách đặt câu hỏi, tiếp nhận feedback, cập nhật tiến độ và phối hợp với đồng đội. Em nhận ra ownership không chỉ là hoàn thành phần code được giao, mà còn là chủ động đưa requirement từ ý tưởng đến kết quả chung của team.

**Chuyển ý:** Từ ba thách thức về công nghệ, kiến trúc và con người, em có ba kết luận chính.

---

## Slide 13 — From Feature Delivery to System Thinking

Kết luận đầu tiên là build with boundaries. Domain rule, module ownership và dependency direction rõ ràng giúp hệ thống dễ thay đổi hơn khi yêu cầu phát triển.

Kết luận thứ hai là design for failure. Retry, idempotency, contract và observability cần được xem là một phần của thiết kế, không phải phần bổ sung sau cùng.

Kết luận thứ ba là prove with feedback. Test, code review và tích hợp với sản phẩm giúp chuyển một giả định kỹ thuật thành bằng chứng có thể kiểm tra.

Bước tiếp theo của em là tiếp tục phát triển khả năng triển khai feature end-to-end, đồng thời đào sâu hơn về architecture, reliability, observability, security và delivery.

Nếu tóm tắt hành trình YPP trong một câu, em sẽ nói rằng: em bắt đầu từ việc tập trung làm cho một chức năng chạy được, và dần học cách suy nghĩ về việc chức năng đó sẽ hoạt động như thế nào trong toàn bộ hệ thống, đặc biệt khi có lỗi xảy ra.

**Chuyển ý:** Phần trình bày của em đến đây là kết thúc.

---

## Slide 14 — Thank You

Cảm ơn mọi người đã lắng nghe.

Em rất mong nhận được câu hỏi và góp ý từ mọi người, đặc biệt về cách em nhìn nhận kiến trúc, những phần kỹ thuật em đã thực hiện, và các hướng học tập tiếp theo.

---

## Câu trả lời gợi ý cho phần Q&A

### 1. Vì sao dự án dùng Modular Monolith thay vì Microservices?

Ở quy mô hiện tại, Modular Monolith giúp team giữ deployment và vận hành đơn giản hơn, nhưng vẫn tạo được boundary giữa các domain. Khi một module thực sự cần scale hoặc deploy độc lập, boundary hiện có sẽ hỗ trợ việc tách service sau này. Tuy nhiên, việc tách không nên thực hiện chỉ vì xu hướng, mà cần dựa trên nhu cầu vận hành thực tế.

### 2. Vì sao phải dùng RabbitMQ cho AI processing?

AI processing có latency cao và có thể gặp lỗi tạm thời. Nếu xử lý trực tiếp trong HTTP request, người dùng phải chờ lâu và request dễ timeout. RabbitMQ giúp đưa tác vụ vào queue, cho phép retry, kiểm soát tải và giữ Web API responsive.

### 3. Phần nào khó nhất đối với bạn?

Phần khó nhất là đảm bảo một flow hoạt động xuyên qua nhiều boundary: API, database, message broker, Java worker và frontend. Việc sửa một model hoặc trạng thái ở một nơi có thể ảnh hưởng nhiều thành phần khác. Điều này khiến em phải chú ý hơn đến contract, idempotency và integration test.

### 4. Bạn tự hào nhất về đóng góp nào?

Em tự hào nhất về việc tham gia xây dựng nền tảng .NET backend và phát triển Identity cùng Jobs Module theo hướng có boundary rõ ràng. Những phần này giúp em đi qua gần như toàn bộ vòng đời của một feature: domain, application logic, persistence, API, messaging và testing.

### 5. Nếu có thêm thời gian, bạn sẽ cải thiện điều gì?

Em muốn cải thiện observability xuyên suốt hệ thống bằng distributed tracing và các metric gắn với business flow. Em cũng muốn thực hiện load test cho luồng import và screening để xác định bottleneck, sau đó kiểm tra lại retry, concurrency và khả năng phục hồi khi một service tạm thời không hoạt động.

### 6. Bạn đã áp dụng Clean Architecture như thế nào?

Em tách domain rule khỏi infrastructure, biểu diễn use case bằng command hoặc query, sử dụng interface cho repository, và đặt implementation của persistence hoặc messaging ở infrastructure layer. Mục tiêu không phải tạo thật nhiều layer, mà là giữ dependency đi đúng hướng và giúp phần business logic dễ test hơn.

### 7. Bài học lớn nhất từ code review là gì?

Bài học lớn nhất là code chạy đúng chưa chắc đã dễ bảo trì. Review giúp em chú ý hơn đến naming, duplication, validation, encapsulation, query tracking, error handling và việc một abstraction có thực sự cần thiết hay không.
