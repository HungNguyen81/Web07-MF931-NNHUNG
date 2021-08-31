<template>
  <div
    :class="['container', { open: isOpen, close: !isOpen }]"
    id="container"
    @click="closeForm(isChange)"
  >
    <div
      :class="['form-container', { open: isOpen, close: !isOpen }]"
      id="form-container"
      @click.stop="formClick"
    >
      <div class="form-header" id="form-container-header">
        <div class="title-header">
          <div class="form-title">Thông tin nhân viên</div>
          <div class="options">
            <div class="option" @click="detail.IsCustomer = !detail.IsCustomer">
              <span class="checkbox-container">
                <input
                  type="checkbox"
                  name="isCustomer"
                  v-model="detail.IsCustomer"
                />
                <div class="checkmark"></div>
              </span>
              <span class="label">Là khách hàng</span>
            </div>
            <div class="option" @click="detail.IsProvider = !detail.IsProvider">
              <span class="checkbox-container">
                <input
                  type="checkbox"
                  name="isProvider"
                  v-model="detail.IsProvider"
                />
                <div class="checkmark"></div>
              </span>
              <span class="label">Là nhà cung cấp</span>
            </div>
          </div>
        </div>
        <div class="header-btn">
          <div class="help-btn form-header-btn" title="Giúp"></div>
          <div
            class="close-btn form-header-btn"
            title="Đóng"
            @click="closeForm(isChange)"
          ></div>
        </div>
      </div>
      <div class="form-body">
        <!-- secion 1 -->
        <div class="form-section form-section-1">
          <!-- Column 1 -->
          <div class="section-column section-column-1">
            <div class="input-row">
              <BaseTextInput
                :valueType="'text'"
                type="input-form"
                inputKey="employee-code"
                :tabindex="1"
                ref="employeeCode"
                v-model="detail.EmployeeCode"
                label="Mã"
                :required="true"
                :validates="[required]"
                :rerenderFlag="isRerender"
                @valid="validateForm"
              />
              <BaseTextInput
                v-model="detail.FullName"
                :valueType="'text'"
                type="input-form"
                inputKey="fullname"
                :tabindex="2"
                label="Tên"
                :required="true"
                ref="fullName"
                :validates="[required]"
                :rerenderFlag="isRerender"
                @valid="validateForm"
              />
            </div>
            <div class="input-row">
              <div class="input-field" id="unit">
                <div class="input-label">
                  Đơn vị
                  <span v-if="required"><span class="required">*</span></span>
                </div>

                <Combobox
                  :mode="0"
                  :api="`${$config.BASE_API}/Units`"
                  :type="'Unit'"
                  :typeDataKey="'UnitName'"
                  :tabindex="5"
                  :label="'Đơn vị'"
                  :validates="[required]"
                  @itemChange="dropDataChange"
                  ref="unitName"
                  @valid="validateForm"
                  v-model="detail.UnitName"
                >
                </Combobox>
              </div>
            </div>
            <div class="input-row">
              <BaseTextInput
                v-model="detail.PositionName"
                :valueType="'text'"
                type="input-form"
                inputKey="position"
                :tabindex="8"
                label="Chức danh"
              />
            </div>
          </div>
          <!-- Column 2 -->
          <div class="section-column section-column-2">
            <div class="input-row">
              <BaseDateInput
                :type="'input-form'"
                inputKey="date-of-birth"
                ref="dateOfBirth"
                :tabindex="3"
                v-model="detail.DateOfBirth"
                :label="'Ngày sinh'"
                :validates="[date, dateNotExceedToday]"
                :rerenderFlag="isRerender"
                @valid="validateForm"
                @dateChange="dateChange"
              />
              <div class="input-field" id="gender">
                <div class="input-label">Giới tính</div>

                <div class="input">
                  <label class="icontainer"
                    >Nam
                    <input
                      :tabindex="4"
                      type="radio"
                      value="1"
                      name="gender"
                      v-model="detail.Gender"
                    />
                    <span class="radio-checkmark"></span>
                  </label>
                  <label class="icontainer"
                    >Nữ
                    <input
                      :tabindex="4"
                      type="radio"
                      value="0"
                      name="gender"
                      v-model="detail.Gender"
                    />
                    <span class="radio-checkmark"></span>
                  </label>
                  <label class="icontainer"
                    >Khác
                    <input
                      :tabindex="4"
                      type="radio"
                      value="2"
                      name="gender"
                      v-model="detail.Gender"
                    />
                    <span class="radio-checkmark"></span>
                  </label>
                </div>
              </div>
            </div>
            <div class="input-row">
              <BaseTextInput
                v-model="detail.IdentityNumber"
                :valueType="'text'"
                type="input-form"
                inputKey="identity-number"
                :tabindex="6"
                label="Số CMND"
                :rerenderFlag="isRerender"
                :pattern="/[0-9]/"
              />
              <BaseDateInput
                v-model="detail.IdentityDate"
                :type="'input-form'"
                inputKey="identity-date"
                :tabindex="7"
                :label="'Ngày cấp'"
                :validates="[date, dateNotExceedToday]"
                :rerenderFlag="isRerender"
                @dateChange="dateChange"
                @valid="validateForm"
                ref="identityDate"
              />
            </div>
            <div class="input-row">
              <BaseTextInput
                v-model="detail.IdentityPlace"
                :valueType="'text'"
                type="input-form"
                inputKey="identity-place"
                :tabindex="9"
                label="Nơi cấp"
              />
            </div>
          </div>
        </div>
        <div class="form-section form-section-2">
          <div class="">
            <div class="input-row">
              <BaseTextInput
                v-model="detail.Address"
                :valueType="'text'"
                type="input-form"
                inputKey="address"
                :tabindex="10"
                label="Địa chỉ"
              />
            </div>
            <div class="input-row">
              <BaseTextInput
                v-model="detail.MobileNumber"
                :valueType="'text'"
                type="input-form"
                inputKey="mobile-number"
                :tabindex="11"
                label="ĐT di động"
                ref="mobileNumber"
                :pattern="/[0-9]/"
              />
              <BaseTextInput
                v-model="detail.PhoneNumber"
                :valueType="'text'"
                type="input-form"
                inputKey="phone-number"
                :tabindex="12"
                label="ĐT cố định"
                ref="phoneNumber"
                :pattern="/[0-9()-]/"
              />
              <BaseTextInput
                v-model="detail.Email"
                :valueType="'text'"
                type="input-form"
                inputKey="email"
                :tabindex="13"
                label="Email"
                ref="email"
                :rerenderFlag="isRerender"
                :validates="[email]"
                @valid="validateForm"
              />
            </div>
            <div class="input-row">
              <BaseTextInput
                v-model="detail.BankAccount"
                :valueType="'text'"
                type="input-form"
                inputKey="bank-account"
                :tabindex="14"
                label="Tài khoản ngân hàng"
                :pattern="/[0-9]/"
              />
              <BaseTextInput
                v-model="detail.BankName"
                :valueType="'text'"
                type="input-form"
                inputKey="bank-name"
                :tabindex="15"
                label="Tên ngân hàng"
              />
              <BaseTextInput
                v-model="detail.BankBranch"
                :valueType="'text'"
                type="input-form"
                inputKey="bank-branch"
                :tabindex="16"
                label="Chi nhánh"
              />
            </div>
          </div>
        </div>
      </div>

      <div class="form-footer">
        <!-- <div class="button  cancel-btn">Hủy</div> -->
        <BaseButtonIcon
          :value="'Hủy'"
          class="form-btn"
          :type="'cancel-btn'"
          :onclick="() => {closeForm(isChange)}"
          :disable="isDisableSaveButton"
          tabindex="17"
        ></BaseButtonIcon>
        <div class="action-btns">
          <BaseButtonIcon
            class="form-btn"
            :value="'Cất'"
            :type="'save-btn'"
            :onclick="
              () => {
                btnSaveClick(false);
              }
            "
            :disable="isDisableSaveButton"
            tabindex="17"
          ></BaseButtonIcon>
          <BaseButtonIcon
            class="form-btn"
            :value="'Cất và thêm'"
            :type="'save-add-btn'"
            :onclick="
              () => {
                btnSaveClick(true);
              }
            "
            :disable="isDisableSaveButton"
            tabindex="17"
          ></BaseButtonIcon>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import EventBus from "../../event-bus/EventBus";
import axios from "axios";
import ultis from "../../mixins/ultis";
import validate from "../../mixins/validate";
import BaseButtonIcon from "../base/BaseButtonIcon.vue";
// import BaseDropdown from "../base/BaseDropdown.vue";
import BaseTextInput from "../base/BaseTextInput.vue";
import BaseDateInput from "../base/BaseDateInput.vue";
import Combobox from "../base/BaseCombobox.vue";

const initObject = {
  IsCustomer: false,
  IsProvider: false,
  Gender: 2,
};

export default {
  name: "Form",
  components: {
    BaseButtonIcon,
    // BaseDropdown,
    BaseTextInput,
    BaseDateInput,
    Combobox,
  },
  mixins: [ultis, validate],
  props: {
    isOpen: {
      type: Boolean,
      required: true,
    },
    close: {
      type: Function,
      require: false,
    },
    mode: {
      type: Number,
      required: false,
      default() {
        return 0;
      }, // 0: For POST action, 1: For PUT action
    },
    detailId: {
      type: String,
      required: false,
    },
    moreDetail: {
      type: Object,
      required: false,
    },
  },
  data() {
    return {
      detail: {},
      initDetail: {},
      isDetailChange: false,
      isDataLoaded: false,
      isRerender: false,
      isDisableSaveButton: false,
      validate: {
        "employee-code": false,
        fullname: false,
        UnitName: false,
        email: false,
        "date-of-birth": false,
        "identity-date": false,
      },
      validateRefs: [
        "employeeCode",
        "fullName",
        "dateOfBirth",
        "unitName",
        "identityDate",
        "phoneNumber",
        "mobileNumber",
        "email",
      ],
      dateMap: {
        "date-of-birth": "DateOfBirth",
        "identity-date": "IdentityDate",
      },
      dateName: {
        "date-of-birth": "Ngày sinh",
        "identity-date": "Ngày cấp",
      },
      dateInputFormat: "yyyy-mm-dd",
    };
  },
  mounted() {},
  computed: {
    isChange: function () {
      return (
        Object.entries(this.initDetail).toString() !=
        Object.entries(this.detail).toString()
      );
    },
  },
  watch: {
    /**
     * Watch sự thay đổi của isOpen để
     * - Auto focus vào trường mã nhân viên khi mở form
     * - Gọi API lấy thông tin đối tượng nếu form mode = 0 <sửa>
     * CreatedBy: HungNguyen81 (07-2021)
     */
    isOpen: function (val) {
      this.$nextTick(() => {
        // tự động focus vào ô mã nv
        if (val) this.$refs.employeeCode.$el.children[1].focus();

        // khởi tạo các giá trị ban đầu => so sánh khi đóng form
        if (this.mode == this.$config.FORM_ADD || this.mode == this.$config.FORM_CLONE) {
          this.initDetail = Object.assign({}, initObject);
          this.detail = Object.assign({}, initObject);
        }
        this.isDisableSaveButton = false;
      });

      this.isDataLoaded = false;
      this.isRerender = !this.isRerender;

      console.log("form " + (val ? "open" : "close"), this.mode);

      if (this.isOpen) {
        console.log(
          "open",
          this.mode,
          this.detailId,
          `${this.$config.BASE_API}/Employees/${this.detailId}`
        );

        // Nếu mode là FORM_UPDATE: sửa nv
        if (this.mode == this.$config.FORM_UPDATE && this.detailId) {
          this.generateUpdateForm();
        }
        // Nếu mode là FORM_ADD: thêm mới nv
        else if (this.mode == this.$config.FORM_ADD) {
          this.generateAddForm();
        } else if (this.mode == this.$config.FORM_CLONE) {
          this.generateUpdateForm(this.getNewCode);
        }
      }
    },
  },
  methods: {
    closeForm(isChange) {
      this.close(isChange, this.mode, this.detailId, this.getRawData());
    },

    formClick(event) {
      EventBus.$emit("appClick", event.target);
    },

    generateUpdateForm: function (callback) {
      axios
        .get(`${this.$config.BASE_API}/Employees/${this.detailId}`)
        .then((res) => {
          if (res.data.IsValid === false) {
            this.$emit("showToast", "warning", "NO Content", res.data.Msg);
            return;
          }

          this.detail = Object.assign({}, res.data);
          console.table(res.data);

          console.groupCollapsed("Data form");
          console.table(res.data);
          console.groupEnd();

          this.formatData();

          this.initDetail = Object.assign({}, this.detail);
          this.isDataLoaded = true;

          if(callback) callback();
        })
        .catch((err) => {
          console.log(err);
        });
    },

    generateAddForm: function () {
      this.isDataLoaded = true;

      this.getNewCode();
    },

    getNewCode() {
      axios
        .get(`${this.$config.BASE_API}/Employees/NewEmployeeCode`)
        .then((res) => {
          let newCode = res.data.Data;
          this.$refs.employeeCode.$el.value = newCode;
          if (!newCode) {
            this.$emit(
              "showToast",
              "error",
              "GET error",
              `Không thể lấy mã nhân viên mới !`
            );
          }
          this.$set(this.detail, "EmployeeCode", newCode);

          this.initDetail = Object.assign({}, this.detail);
        })
        .catch(() => {
          this.$emit(
            "showToast",
            "error",
            "GET error",
            `Không thể lấy mã nhân viên mới !`
          );
          let newCode = `NV-${Math.round(Math.random() * 10000)}`;
          this.$refs.employeeCode.$el.value = newCode;
          this.$set(this.detail, "EmployeeCode", newCode);

          this.initDetail = Object.assign({}, this.detail);
        });
    },
    /**
     * Handle mỗi khi date input value thay đổi
     * CreatedBy: HungNguyen81 (07-2021)
     */
    dateChange(key, val, input, formatedVal) {
      let keyName = this.dateMap[key];
      let oldVal = this.detail[keyName];
      if (oldVal != val && val) {
        // validate định dạng ngày
        !this.date(this.dateName[key], formatedVal);

        let start = input.selectionStart;
        // console.log("start", start, ", date", val, formatedVal);
        this.$set(this.detail, keyName, val);
        this.$nextTick(() => {
          input.setSelectionRange(start + 1, start + 1);
        });
      }
    },

    /**
     * Format dữ liệu để hiển thị
     * CreatedBy: HungNguyen81 (07-2021)
     */
    formatData() {
      this.$set(
        this.detail,
        "DateOfBirth",
        this.dateFormatVer2(this.detail.DateOfBirth, this.dateInputFormat)
      );
      this.$set(
        this.detail,
        "IdentityDate",
        this.dateFormatVer2(this.detail.IdentityDate, this.dateInputFormat)
      );
      if (this.detail.Gender === null) {
        this.$set(this.detail, "Gender", 2);
      }
    },

    /**
     * Kiểm tra tính hợp lệ của các trường dữ liệu trên form
     * CreatedBy: HungNguyen81 (07-2021)
     */
    isValidate() {
      let res = true;
      Object.keys(this.validate).forEach((key) => {
        res = res && this.validate[key];
      });
      console.groupCollapsed("Validate info");
      console.table(JSON.parse(JSON.stringify(this.validate)));
      console.groupEnd();
      return res;
    },

    /**
     * Lấy dữ liệu thô để post/put
     * CreatedBy: HungNguyen81 (07-2021)
     */
    getRawData() {
      let dob = this.detail.DateOfBirth;
      let identityDate = this.detail.IdentityDate;
      let res = JSON.parse(JSON.stringify(this.detail));

      res.DateOfBirth = dob ? new Date(dob).toISOString() : null;
      res.IdentityDate = identityDate
        ? new Date(identityDate).toISOString()
        : null;
      res.Gender = Number(this.detail.Gender);
      return res;
    },

    /**
     * Handle khi click nút lưu
     * CreatedBy: HungNguyen81 (07-2021)
     */
    btnSaveClick(isAddNext) {
      this.isDisableSaveButton = true;
      var invalidMsg = "";
      var isInvalid = false;
      for (let ref of this.validateRefs) {
        let valid = this.$refs[ref].inputValidate();

        // lấy thông báo ko hợp lệ đầu tiên để hiện thị lên
        if (!valid.IsValid && !isInvalid) {
          invalidMsg = valid.Msg;
          isInvalid = true;
        }
      }
      if (!this.isValidate()) {
        this.$emit("showToast", "warning", "Dữ liệu không hợp lệ", invalidMsg);
        this.$emit("showPopup", {
          content: invalidMsg,
          popupType: "error",
          isHide: false,
          buttons: [{ type: "button-ok", callback: null, value: "Đóng" }],
        });
        this.isDisableSaveButton = false;
        return;
      }

      var func = () => {
        this.initDetail = Object.assign({}, initObject);
        this.detail = Object.assign({}, initObject);
        this.getNewCode();
        this.isDisableSaveButton = false;
        this.$nextTick(() => {
          // tự động focus vào ô mã nv
          this.$refs.employeeCode.$el.children[1].focus();
        });
      };

      // nếu thông tin đã thay đổi
      if (this.isChange) {
        this.isDisableSaveButton = true;
        this.$emit(
          "saveClicked",
          this.mode,
          this.detailId,
          this.getRawData(),
          isAddNext ? func : ()=>{this.closeForm(false);}
        );
        EventBus.$on('requestFail', () =>{
          this.isDisableSaveButton= false;
        })

      } else {
        if (!isAddNext) {
          this.closeForm(false);
        } else {
          func();
        }
      }
    },

    /**
     * Khi select trong dropdown, chỉ có DepartmentName thay đổi mà DepartmentId không thay đổi
     * Tương tự với PositionId
     * CreatedBy: HungNguyen81 (07-2021)
     */
    dropDataChange(typeName, obj) {
      this.$set(this.detail, typeName, obj[typeName]);

      var key = typeName.split("Name")[0] + "Id";

      if (obj[key]) {
        this.$set(this.detail, key, obj[key]);
      }
    },

    /**
     * Emit sự kiện showToast cho parent(base content)
     * CreatedBy: HungNguyen81 (07-2021)
     */
    emitShowToast(type, header, msg) {
      this.$emit("showToast", type, header, msg);
    },

    /**
     * Handle khi validate input
     * CreatedBy: HungNguyen81 (07-2021)
     */
    validateForm(key, isValid) {
      this.$set(this.validate, key, isValid);
    },
  },
};
</script>

<style scoped>
@import "../../css/components/popup-form.css";
@import "../../css/base/checkbox.css";
@import "../../css/base/radio.css";
</style>