# Mẫu Prompt Chuẩn Cho Subagent Nghiên Cứu (Dễ Hiểu Cho Người Mới & Low-Tech)

Khi nhận được yêu cầu tìm hiểu bất kỳ chủ đề lập trình hoặc công nghệ nào, hãy cấu trúc prompt cho subagent theo 6 trục nghiên cứu dưới đây, **ưu tiên tính dễ hiểu, ngôn ngữ đời thường và ví dụ trực quan** bên cạnh độ chính xác kỹ thuật:

```markdown
Hãy nghiên cứu toàn diện về chủ đề: `{CHỦ_ĐỀ_HOẶC_TỪ_KHÓA}` trong bối cảnh `{CÔNG_NGHỆ_HOẶC_ENGINE}` (dựa trên Tài liệu chính thức, Kinh nghiệm thực tế, Best Practices & Anti-patterns).

**YÊU CẦU QUAN TRỌNG VỀ CÁCH DIỄN ĐẠT:**
- Người đọc mục tiêu bao gồm cả người **Low-tech hoặc mới tiếp cận công nghệ này**.
- Tuyệt đối KHÔNG nhồi nhét thuật ngữ tiếng Anh chuyên ngành nếu có thể diễn đạt bằng tiếng Việt bình dân.
- Mọi thuật ngữ kỹ thuật bắt buộc phải dùng (như `Static`, `Heap`, `Garbage Collector`, `Serialization`, `Instance`, `Async`...) phải đi kèm **1 câu giải thích bình dân** ("hiểu đơn giản là...") và **1 ví dụ ẩn dụ đời thường** (ví dụ: bảng thông báo chung, nhân viên dọn rác, bản vẽ thiết kế vs ngôi nhà thật...).
- Tách bạch rõ phần **Kiến thức cốt lõi dễ hiểu** (ai cũng đọc được) và phần **Chi tiết kỹ thuật nâng cao** (để người đọc có thể bỏ qua nếu chưa cần).

Nội dung nghiên cứu cần tập trung vào 6 trục:
1. **Giải thích bình dân & Từ điển thuật ngữ nhanh:**
   - Chủ đề này giải quyết vấn đề gì trong thực tế? (1-2 câu ngắn gọn, không thuật ngữ).
   - 1 ví dụ ẩn dụ đời thường xuyên suốt giúp người chưa biết lập trình cũng hình dung được ngay.
   - Bảng giải thích 5-8 thuật ngữ cốt lõi sẽ xuất hiện trong bài (viết bằng ngôn ngữ đời thường, không dùng thuật ngữ này để giải thích thuật ngữ kia).
2. **Bản chất & Cách hoạt động (Từ cơ bản đến nâng cao):**
   - Cách hoạt động cốt lõi giải thích bằng ngôn ngữ tự nhiên, từng bước một.
   - Chi tiết kỹ thuật sâu hơn (bộ nhớ, vòng đời, cơ chế bên dưới của trình biên dịch/runtime) — đánh dấu rõ là phần nâng cao.
3. **Đặc thù riêng trong Engine / Framework:**
   - Khi đưa vào môi trường thực tế (ví dụ: Unity Game Loop, chuyển Scene, bấm Play/Stop trong Editor) thì có điều gì bất ngờ hay xảy ra?
   - Giải thích rõ *tại sao* lại xảy ra bằng ngôn ngữ dễ hiểu trước khi gọi tên kỹ thuật.
4. **Ưu điểm, Nhược điểm & Khi nào nên dùng (Ma trận quyết định):**
   - Ưu điểm vs Rủi ro thường gặp (diễn đạt bằng hệ quả thực tế: "làm game nặng dần theo thời gian", "sửa chỗ này làm hỏng chỗ khác"... thay vì chỉ ghi từ khóa như "Memory Leak", "Tight Coupling").
   - Khi nào NÊN dùng vs KHÔNG NÊN dùng (kèm câu hỏi tự kiểm tra đơn giản dạng Có/Không).
5. **Bộ Code Mẫu Thực Chiến (Giải thích từng dòng):**
   - Cung cấp 3-4 cặp ví dụ: Cách làm dễ gây lỗi (❌) đối chiếu với Cách làm an toàn (✅).
   - Bắt buộc có **comment tiếng Việt dễ hiểu** ở từng bước quan trọng trong code, và giải thích hậu quả thực tế nếu làm sai.
6. **Ý tưởng sơ đồ trực quan hóa dễ hiểu:**
   - Gợi ý 2-3 sơ đồ (ví dụ: so sánh trực quan, luồng đi từng bước, cây quyết định Có/Không).
   - Nhãn trong sơ đồ phải dùng **tiếng Việt dễ hiểu**, gắn với ví dụ đời thường, tránh nhồi nhét thuật ngữ tiếng Anh khó hiểu.

Hãy tổng hợp kết quả rõ ràng, mạch lạc, ưu tiên sự dễ hiểu cho người mới nhưng vẫn đảm bảo chính xác về mặt kỹ thuật.
```
