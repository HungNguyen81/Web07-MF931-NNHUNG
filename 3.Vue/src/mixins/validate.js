export default {
    methods: {
        /**
         * Kiểm tra các trường bắt buộc nhập không được rỗng
         * @param {String} label Tên trường dữ liệu
         * @param {String} val Giá trị
         * @returns true, false
         * CreatedBy: HungNguyen81 (18-08-2021)
         * ModifiedBy: HungNguyen81 (18-08-2021)
         */
        required(label, val) {
            if (!val || !val.trim()) {
                // this.$emit('showToast', 'warning', 'Required', `<b>"${label}"</b> không được để trống`);
                return {
                    isValid: false,
                    msg: this.$resourceVn.InvalidRequiredMsg.replace('@', label)
                };
            }
            return {
                isValid: true,
                msg: ""
            };
        },
        
        /**
         * Validate định dạng email
         * @param {String} label tên input
         * @param {String} val Giá trị input
         * @returns True-False
         * CreatedBy: HungNguyen81 (18-08-2021)
         * ModifiedBy: HungNguyen81 (18-08-2021)
         */
        email(label, val) {
            if(!val) return {
                isValid: true,
                msg: ""
            };
            let reg = /\S+@\S+\.\S+/;
            let res = reg.test(val);
            return {
                isValid: res,
                msg: res? "": this.$resourceVn.InvalidEmailMsg.replace('@', label)
            };
        },

        /**
         * Validate Ngày tháng
         * @param {string} label tên của trường dữ liệu ngày tháng
         * @param {string} dateString giá trị ngày tháng cần validate
         * @returns true- hợp lệ, false- không hợp lệ
         * CreatedBy: HungNguyen81 (18-08-2021)
         * ModifiedBy: HungNguyen81 (18-08-2021)
         */
        date(label, dateString) {
            if(!dateString){
                return {
                    isValid : true,
                    msg: ""
                };
            }
            if (!/^([0-9]{4})-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$/g.test(dateString)) {
                return {
                    isValid: false,
                    msg: this.$resourceVn.InvalidDateMsg.replace('@', label).replace('#', dateString)
                };
            }
            return {
                isValid : true,
                msg: ""
            };
        },
        dateNotExceedToday(label, dateString){
            let valid = this.date(label, dateString)
            if(valid.isValid){
                var today = new Date();
                var dateVal = new Date(dateString);
                if(dateVal > today) return {
                    isValid: false,
                    msg: this.$resourceVn.InvalidDayNotExceedTodayMsg.replace('@', label)
                }
                else return {isValid: true, msg: ''};
            } else {
                return valid;
            }
        }
    }
}