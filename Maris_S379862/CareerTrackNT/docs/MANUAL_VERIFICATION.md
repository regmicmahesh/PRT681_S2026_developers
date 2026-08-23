# Manual Verification

Record tester/date and actual result only after running each check.

| Check | Expected | Actual/date |
|---|---|---|
| Start app and open `/` | Page loads without missing static files | Pending human browser check |
| Keyboard Tab from top | Skip link appears; focus order is logical and visible | Pending |
| Save valid application | Success announced; row and counts update | Pending |
| Save blank/invalid fields | Browser/server errors identify fields; no row added | Pending |
| Search/filter | Only matching rows display; empty state is meaningful | Pending |
| Edit | Values load; update persists after refresh | Pending |
| Delete then cancel | Record remains | Pending |
| Delete then confirm | Record removed; success announced | Pending |
| 320px, 768px, 1024px, 1440px widths | Content remains readable and operable | Pending |
| 200% zoom | No loss of content/control | Pending |
| Browser console/network | No unexpected errors; API status/body correct | Pending |

Automated evidence is complementary, not a substitute for the browser/accessibility checks above.
