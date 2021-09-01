<template>
  <div>
    <Header :collapse="collapse" :class="{collapse: isCollapse}"></Header>
    <Menu :route="route" :items="menuItems" :class="{collapse: isCollapse}"></Menu>
    <Content
      :features="features"
      entityName="Employee"
      :filterNames="['DepartmentId', 'PositionId']"
      :headers="headers"
      :class="{collapse: isCollapse}"
    ></Content>
  </div>
</template>

<script>
import Header from "../layout/TheHeader.vue";
import Menu from "../layout/TheMenu.vue";
import Content from "../base/BaseContent.vue";
import axios from "axios";
import ultis from '../../mixins/ultis'

export default {
  name: "EmployeePage",
  components: {
    Header,
    Menu,
    Content,
  },
  mixins:[ultis],
  data() {
    return {
      headers:[
        {key: "EmployeeCode", name: "MÃ NHÂN VIÊN"},
        {key: "FullName", name: "TÊN NHÂN VIÊN"},
        {key: "GenderName", name: "GIỚI TÍNH"},
        {key: "DateOfBirth", name: "NGÀY SINH"},
        {key: "IdentityNumber", name: "SỐ CMND"},
        {key: "PositionName", name: "CHỨC DANH"},
        {key: "UnitCode", name: "MÃ ĐƠN VỊ"},
        {key: "UnitName", name: "TÊN ĐƠN VỊ"},
        {key: "MobileNumber", name: "ĐT di động"},
        {key: "PhoneNumber", name: "ĐT cố định"},
        {key: "Email", name: "Email"},
        {key: "IsCustomer", name: "LÀ KHÁCH HÀNG"},
        {key: "IsProvider", name: "LÀ NHÀ CUNG CẤP"},
        {key: "BankAccount", name: "SỐ TÀI KHOẢN"},
        {key: "BankName", name: "TÊN NGÂN HÀNG"},
        {key: "BankBranch", name: "CHI NHÁNH TK NGÂN HÀNG"},
      ],
      features: [],
      menuItems: [
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
      isCollapse: false,
      route: "/",
    };
  },
  created() {
    this.features.push({
      featureClass: "btn-export",
      action: this.exportEmployees,
    });
  },
  methods: {
    /**
     * Xuất dữ liệu toàn bộ nv ra file excel
     * CreatedBy: HungNguyen81 (31-08-2021)
     */
    exportEmployees() {
      // var dateFormat = "yyyyMMdd-hhmmss";
      var currTimeString = this.dateFormatVer2(new Date(), "yyyyMMddhhmmss");
      document.querySelector('.btn-export').classList.toggle('loading');
      axios
        .get(`${this.$config.BASE_API}/Employees/employeesFile`,{
          responseType: 'blob',
        })
        .then(response => {
          document.querySelector('.btn-export').classList.toggle('loading');
          const url = URL.createObjectURL(new Blob([response.data]));
          const link = document.createElement("a");
          link.href = url;
          link.setAttribute(
            "download",
            `Nhanvien_${currTimeString}.xlsx`
          );
          document.body.appendChild(link);
          link.click();
        });
    },
    collapse(){
      this.isCollapse = !this.isCollapse;
    }
  },
};
</script>

<style>
</style>
