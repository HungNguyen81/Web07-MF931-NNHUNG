module.exports = {
    // header texts
    CompanyName: "CÔNG TY TNHH SẢN XUẤT - THƯƠNG MẠI - DỊCH VỤ QUI PHÚC",

    // messages
    NetworkErrorMsg: "Không thể kết nối đến máy chủ, vui lòng kiểm tra kết nối mạng.",
    DataChangeMsg: "Dữ liệu đã bị thay đổi. Bạn có muốn cất không?",
    EmployeeDeleteMsg: "Bạn có thực sự muốn xóa nhân viên <@> không ?",
    ServerErrorMsg: "Đã có lỗi xảy ra, vui lòng liên hệ MISA !",
    ErrorTitle: "Có lỗi xảy ra",
    NotFoundTitle: "Không tìm thấy",
    EmployeeNotFoundMsg: "Không tìm thấy dữ liệu nhân viên !",
    EmployeeSearchPlaceHolderText: "Tìm kiếm theo mã, tên nhân viên",
    EmployeeButtonAddText: "Thêm mới nhân viên",
    EmployeeButtonDeleteText: "Xóa nhân viên",
    CannotGetNewEmployeeCodeMsg: "Không thể lấy mã nhân viên mới !",
    DataInvalidMsg: "Dữ liệu không hợp lệ",

    // entities
    Customer: "Khách hàng",
    Employee: "Nhân viên",
    Position: "vị trí",
    PageSize: "Kích thước trang",
    Unit: "Đơn vị",

    // Button texts
    NoButtonText: "Không",
    YesButtonText: "Có",
    ConfirmButtonText: "Đồng ý",
    CloseButtonText: "Đóng",
    HelpButtonTitle: "Giúp",
    CloseButtonTitle: "Đóng",
    CancelButtonText: "Hủy",
    SaveButtonText: "Cất",
    SaveAddButtonText: "Cất và thêm",
    NextButtonText: "Trước",
    PrevButtonText: "Sau",

    // popup titles and msg
    DeleteSuccessTitle: "Xóa thành công",
    DeleteFailTitle: "Xóa thất bại",
    UpdateSuccessMsg: "Sửa thông tin của <b>@</b> thành công.",
    PostSuccessMsg: "Thêm mới nhân viên <b>@</b> thành công.",
    PostSuccessTitle: "Thêm mới thành công",
    PutSuccessTitle: "Cập nhật thành công",
    PutErrorTitle: "Sửa thông tin thất bại",
    EmployeeContentHeader: "Thông tin nhân viên",

    // Form Labels:
    IsCustomer: "Là khách hàng",
    IsProvider: "Là nhà cung cấp",
    LabelEmployeeCode: "Mã",
    LabelFullname: "Tên",
    LabelUnit: "Đơn vị",
    LabelPositionName: "Chức danh",
    LabelDateOfBirth: "Ngày sinh",
    LabelGender: "Giới tính",
    OptionMale: "Nam",
    OptionFemale: "Nữ",
    OptionOthers: "Khác",
    LabelIdentityNumber: "Số CMND",
    LabelIdentityDate: "Ngày cấp",
    LabelIdentityPlace: "Nơi cấp",
    LabelAddress: "Địa chỉ",
    LabelMobileNumber: "ĐT di động",
    LabelPhoneNumber: "ĐT cố định",
    LabelEmail: "Email",
    LabelBankAccount: "Tài khoản ngân hàng",
    LabelBankName: "Tên ngân hàng",
    LabelBankBranch: "Chi nhánh",

    LabelTotalRecord: "Tổng số: <b> @ </b> bản ghi",

    // fixed data    
    PageSizeDropData: [
        { PageSizeName: "10 bản ghi trên 1 trang", Size: 10 },
        { PageSizeName: "20 bản ghi trên 1 trang", Size: 20 },
        { PageSizeName: "30 bản ghi trên 1 trang", Size: 30 },
        { PageSizeName: "50 bản ghi trên 1 trang", Size: 50 },
        { PageSizeName: "100 bản ghi trên 1 trang", Size: 100 },
    ],
    TableHeaders: [
        { key: "EmployeeCode", name: "MÃ NHÂN VIÊN" },
        { key: "FullName", name: "TÊN NHÂN VIÊN" },
        { key: "GenderName", name: "GIỚI TÍNH" },
        { key: "DateOfBirth", name: "NGÀY SINH" },
        { key: "IdentityNumber", name: "SỐ CMND" },
        { key: "PositionName", name: "CHỨC DANH" },
        { key: "UnitCode", name: "MÃ ĐƠN VỊ" },
        { key: "UnitName", name: "TÊN ĐƠN VỊ" },
        { key: "MobileNumber", name: "ĐT di động" },
        { key: "PhoneNumber", name: "ĐT cố định" },
        { key: "Email", name: "Email" },
        { key: "IsCustomer", name: "LÀ KHÁCH HÀNG" },
        { key: "IsProvider", name: "LÀ NHÀ CUNG CẤP" },
        { key: "BankAccount", name: "SỐ TÀI KHOẢN" },
        { key: "BankName", name: "TÊN NGÂN HÀNG" },
        { key: "BankBranch", name: "CHI NHÁNH TK NGÂN HÀNG" },
    ],
    MenuItems: [
        { icon: "item-dashboard", path: "#", text: "Tổng quan" },
        { icon: "item-money", path: "#", text: "Tiền mặt" },
        { icon: "item-deposits", path: "#", text: "Tiền gửi" },
        { icon: "item-buy", path: "#", text: "Mua hàng" },
        { icon: "item-sale", path: "#", text: "Bán hàng" },
        { icon: "item-bill", path: "#", text: "Quản lý hóa đơn" },
        { icon: "item-storage", path: "#", text: "Kho" },
        { icon: "item-tool", path: "#", text: "Công cụ hỗ trợ" },
        { icon: "item-asset", path: "#", text: "Tài sản" },
        { icon: "item-tax", path: "#", text: "Thuế" },
        { icon: "item-price", path: "#", text: "Giá thành" },
        { icon: "item-sumup", path: "#", text: "Tổng hợp" },
        { icon: "item-budget", path: "#", text: "Ngân sách" },
        { icon: "item-report", path: "#", text: "Báo cáo" },
        { icon: "item-analysis", path: "#", text: "Phân tích tài chính" },
    ],
    ExportFilePrefix: "Nhanvien",

    InvalidRequiredMsg: `"@" không được để trống.`,
    InvalidEmailMsg: `"@" không đúng định dạng.`,
    InvalidDateMsg: `"@" # không đúng định dạng.`,
    InvalidDayNotExceedTodayMsg: `"@" không được vượt quá ngày hiện tại !`
}