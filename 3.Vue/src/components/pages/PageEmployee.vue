<template>
  <div>
    <Header></Header>
    <Menu :route="route" :items="menuItems"></Menu>
    <Content
      :features="features"
      entityName="Employee"
      :filterNames="['DepartmentId', 'PositionId']"
      :thead="thead"
      :theadMap="theadMap"
    ></Content>
  </div>
</template>

<script>
import Header from "../layout/TheHeader.vue";
import Menu from "../layout/TheMenu.vue";
import Content from "../base/BaseContent.vue";
import axios from "axios";

export default {
  name: "EmployeePage",
  components: {
    Header,
    Menu,
    Content,
  },
  data() {
    return {
      thead: [
        "MÃ NHÂN VIÊN",
        "TÊN NHÂN VIÊN",
        "GIỚI TÍNH",
        "NGÀY SINH",
        "SỐ CMND",
        "CHỨC DANH",
        "TÊN ĐƠN VỊ",
        "SỐ TÀI KHOẢN",
        "TÊN NGÂN HÀNG",
        "CHI NHÁNH TK NGÂN HÀNG",
      ],
      theadMap: [
        "EmployeeCode",
        "FullName",
        "GenderName",
        "DateOfBirth",
        "IdentityNumber",
        "PositionName",
        "UnitName",
        "BankAccount",
        "BankName",
        "BankBranch",
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
    exportEmployees() {
      // var dateFormat = "yyyyMMdd-hhmmss";
      var currTime = new Date();
      var currTimeString = currTime.getSeconds();

      axios
        .get(`${this.$config.BASE_API}/Employees/employeesFile`,{
          responseType: 'blob',
        })
        .then(response => {
          const url = URL.createObjectURL(new Blob([response.data]));
          const link = document.createElement("a");
          link.href = url;
          link.setAttribute(
            "download",
            `Danh_sach_nhan_vien_${currTimeString}.xlsx`
          );
          document.body.appendChild(link);
          link.click();
        });
    },
  },
};
</script>

<style>
</style>
