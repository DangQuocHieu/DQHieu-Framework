# Mẫu Prompt Chuẩn Cho Subagent Nghiên Cứu Kỹ Thuật (Research Subagent)

Khi nhận được yêu cầu tìm hiểu bất kỳ chủ đề lập trình hoặc game dev nào, hãy cấu trúc prompt cho subagent theo 6 trục nghiên cứu trọng tâm dưới đây:

```markdown
Hãy nghiên cứu toàn diện và chuyên sâu về chủ đề: `{CHỦ_ĐỀ_HOẶC_TỪ_KHÓA}` trong bối cảnh `{CÔNG_NGHỆ_HOẶC_ENGINE}` (dựa trên Tài liệu chính thức, Đặc tả kỹ thuật ngôn ngữ, Diễn đàn lập trình thực tế, Best Practices & Anti-patterns).

Nội dung nghiên cứu cần tập trung vào 6 trục:
1. Bản chất kỹ thuật & Cơ chế nền tảng:
   - Định nghĩa chính xác, cú pháp, cách thức trình biên dịch và runtime xử lý.
   - Cơ chế cấp phát bộ nhớ (Stack vs Managed Heap vs Native Heap), vòng đời (Lifetime), hành vi của Garbage Collector (GC).
2. Đặc thù riêng trong Game Engine / Framework:
   - Tương tác với Game Loop / Engine Lifecycle (Awake, Start, Update, Scene Load/Unload, AppDomain Reload, Serialization).
   - Tương tác với đa luồng (Multi-threading, Job System) hoặc xử lý bất đồng bộ (Coroutine, async/await, UniTask).
3. Ưu điểm & Nhược điểm kỹ thuật chuyên sâu:
   - Ưu điểm: Hiệu năng, tiện ích, tối ưu bộ nhớ, tốc độ thực thi.
   - Nhược điểm & Rủi ro: Rò rỉ bộ nhớ (Memory Leak), Tight Coupling, khó Unit Test / Mock, Race Condition, Deadlock, phá vỡ SOLID.
4. Ma trận quyết định (Decision Matrix):
   - Khi nào NÊN sử dụng? (Liệt kê các kịch bản thực tế kèm lý do).
   - Khi nào TUYỆT ĐỐI TRÁNH? (Các cạm bẫy, anti-pattern thường gặp).
   - Phương án thay thế tốt hơn trong từng trường hợp.
5. Bộ Code Mẫu Thực Chiến (Đối Chiếu Sai vs Đúng):
   - Cung cấp ít nhất 3-4 cặp code: Code Sai (Anti-Pattern dễ dính bug) đối chiếu trực tiếp với Code Đúng (Best Practice chuẩn production).
   - Giải thích cặn kẽ tại sao code sai lại sinh ra lỗi và cơ chế đằng sau cách sửa.
6. Ý tưởng trực quan hóa sơ đồ:
   - Gợi ý 3-4 sơ đồ hoặc biểu đồ (Memory layout, Lifecycle flow, Decision tree, Architectural diagram) giúp người đọc dễ hình dung nhất.

Hãy tổng hợp kết quả chi tiết, rõ ràng, có chiều sâu kỹ thuật cao để làm cơ sở biên soạn tài liệu HTML hoàn chỉnh.
```

