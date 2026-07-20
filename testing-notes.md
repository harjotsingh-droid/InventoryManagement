# Testing Notes

## Automated tests

Run from repository root:

```powershell
dotnet test
```

| Test class | Coverage |
|------------|----------|
| `QuotationCalculatorTests` | Line and document totals with discount/GST |
| `SettingsDefaultsTests` | Default settings fallback and stored overrides |

## Manual test checklist

### Authentication
- [ ] Login with admin@demo.com / Admin@123
- [ ] Invalid password shows error
- [ ] Logout returns to login page

### Products
- [ ] Index shows 6 seeded products
- [ ] Search "bolt" returns Steel Bolt M8
- [ ] Create product with duplicate SKU rejected
- [ ] Edit product persists after refresh

### Customers
- [ ] Index shows 4 seeded customers
- [ ] Create customer with required fields

### Quotations
- [ ] Create quotation with 2+ line items
- [ ] Detail page shows correct totals
- [ ] Download PDF opens with company address and color
- [ ] Missing customer rejected
- [ ] Zero quantity rejected

### Settings
- [ ] Change address and primary color, save
- [ ] Dashboard/nav accent updates (may need refresh)
- [ ] New PDF reflects updated address and color

### Global search
- [ ] Query "alpha" finds customer
- [ ] Query "QT" finds quotations
- [ ] Single character shows validation message

### Persistence
- [ ] Stop and restart app — data remains

## Known limitations

- Theme color in layout requires authenticated session (ViewComponent)
- No automated UI/integration tests in core scope
