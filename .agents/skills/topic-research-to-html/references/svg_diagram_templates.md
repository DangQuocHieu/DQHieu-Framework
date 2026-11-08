# Thư Viện Mẫu Sơ Đồ SVG Trực Quan Cho Tài Liệu Kỹ Thuật

Các mẫu sơ đồ SVG dưới đây được tối ưu hóa cho giao diện kỹ thuật hiện đại (hỗ trợ Dark Mode, responsive qua `viewBox`, bảng màu Cyberpunk/Clean Slate: Blue `#38bdf8`, Purple `#a855f7`, Green `#10b981`, Red `#f43f5e`, Yellow `#fbbf24`).

---

## 1. Mẫu Sơ Đồ Cấp Phát Bộ Nhớ (Memory Allocation / GC Roots)
```xml
<div class="diagram-container">
    <div class="diagram-title"><span>📊 Sơ Đồ Phân Bổ Bộ Nhớ</span></div>
    <svg class="diagram-svg" viewBox="0 0 850 300" width="850" height="300" xmlns="http://www.w3.org/2000/svg">
        <rect x="10" y="10" width="830" height="280" rx="10" fill="#0f172a" stroke="#222f49" stroke-width="1.5"/>
        <text x="35" y="40" fill="#94a3b8" font-family="'JetBrains Mono', monospace" font-size="13" font-weight="bold">Process Memory Boundary</text>
        
        <!-- Left: Static / Type Area -->
        <rect x="35" y="65" width="360" height="205" rx="8" fill="#1e293b" stroke="#38bdf8" stroke-width="1.5"/>
        <text x="55" y="95" fill="#38bdf8" font-family="'Plus Jakarta Sans', sans-serif" font-size="13" font-weight="700">Vùng Nhớ Cố Định (Static Storage / Type Descriptor)</text>
        <rect x="55" y="120" width="320" height="40" rx="4" fill="#0b0f19" stroke="#38bdf8"/>
        <text x="70" y="145" fill="#cbd5e1" font-family="'JetBrains Mono', monospace" font-size="11">Primitive Types (Value types)</text>
        <rect x="55" y="170" width="320" height="40" rx="4" fill="#0b0f19" stroke="#a855f7"/>
        <text x="70" y="195" fill="#c084fc" font-family="'JetBrains Mono', monospace" font-size="11">GC Root References (Pointers)</text>

        <!-- Right: Managed Heap Area -->
        <rect x="445" y="65" width="370" height="205" rx="8" fill="#1e293b" stroke="#10b981" stroke-width="1.5"/>
        <text x="465" y="95" fill="#34d399" font-family="'Plus Jakarta Sans', sans-serif" font-size="13" font-weight="700">Managed Heap (GC Tracked)</text>
        <rect x="465" y="140" width="330" height="60" rx="4" fill="#0b0f19" stroke="#a855f7" stroke-width="1.5"/>
        <text x="480" y="168" fill="#e2e8f0" font-family="'JetBrains Mono', monospace" font-size="11">Active Payload Objects</text>
        <text x="480" y="188" fill="#f43f5e" font-family="'Plus Jakarta Sans', sans-serif" font-size="10">→ Giữ xích bởi GC Root pointer</text>

        <!-- Connection Arrow -->
        <path d="M 375 190 L 465 170" fill="none" stroke="#a855f7" stroke-width="2.5" marker-end="url(#arrow-p)"/>
        <defs>
            <marker id="arrow-p" markerWidth="8" markerHeight="8" refX="6" refY="4" orient="auto">
                <path d="M 0 0 L 8 4 L 0 8 z" fill="#a855f7" />
            </marker>
        </defs>
    </svg>
</div>
```

---

## 2. Mẫu Sơ Đồ Cây Quyết Định (Decision Tree Flowchart)
```xml
<div class="diagram-container">
    <div class="diagram-title"><span>🧭 Sơ Đồ Cây Quyết Định (Decision Tree)</span></div>
    <svg class="diagram-svg" viewBox="0 0 850 360" width="850" height="360" xmlns="http://www.w3.org/2000/svg">
        <rect x="10" y="10" width="830" height="340" rx="10" fill="#0f172a" stroke="#222f49" stroke-width="1.5"/>
        
        <!-- Start Block -->
        <rect x="300" y="30" width="250" height="40" rx="6" fill="#1e293b" stroke="#38bdf8" stroke-width="2"/>
        <text x="425" y="55" fill="#38bdf8" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="12" font-weight="bold">Yêu cầu thiết kế tính năng mới</text>
        
        <!-- Decision Rhombus -->
        <polygon points="425,100 550,140 425,180 300,140" fill="#152438" stroke="#a855f7" stroke-width="1.5"/>
        <text x="425" y="145" fill="#e2e8f0" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="11">Có cần lưu State?</text>

        <!-- Branch 1: No -->
        <rect x="640" y="120" width="160" height="45" rx="6" fill="#0d2b24" stroke="#10b981" stroke-width="1.5"/>
        <text x="720" y="147" fill="#34d399" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="11" font-weight="bold">DÙNG STATIC / UTILITY</text>

        <!-- Branch 2: Yes -->
        <rect x="345" y="230" width="160" height="45" rx="6" fill="#2d121c" stroke="#f43f5e" stroke-width="1.5"/>
        <text x="425" y="257" fill="#fb7185" text-anchor="middle" font-family="'Plus Jakarta Sans', sans-serif" font-size="11" font-weight="bold">DÙNG COMPONENT / SO</text>
    </svg>
</div>
```

