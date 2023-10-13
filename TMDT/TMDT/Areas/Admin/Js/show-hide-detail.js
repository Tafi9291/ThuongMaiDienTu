// Lựa chọn để xuất hiện bảng thông tin chi tiết sản phẩm theo ngành hàng

const dienthoaiDetails = document.getElementById('dienthoai-details');
const buttonSave = document.getElementById('button-save');

const selectCategories = document.getElementById('tafi-select-categories');
const panelTitle = document.getElementById('panel-title');
const panelContentWrapper = document.querySelector('#panel-content-price');
const panelInactiveContent = document.querySelector('.panel-inactive-content');

const sectionBasicInfo = document.getElementById('section-basic-info');

function removeElements() {
    if (panelInactiveContent) {
        panelInactiveContent.remove();
    }
    if (panelTitle.classList.contains('inactive')) {
        panelTitle.classList.remove('inactive');
    }
    if (buttonSave.classList.contains('disabled')) {
        buttonSave.classList.remove('disabled');
    }

}

function addElements() {
    if (!panelInactiveContent) {
        panelInactiveContent.add();
    }
    if (!panelTitle.classList.contains('inactive')) {
        panelTitle.classList.add('inactive');
    }
    if (!buttonSave.classList.contains('disabled')) {
        buttonSave.classList.add('disabled');
    }

}

// Khai báo một biến để lưu trạng thái chi tiết
let isDetailDienThoaiVisible = false;

function detailDienThoai() {

    const newSectionDetailInfo = document.createElement('section')
    newSectionDetailInfo.className = 'product-edit__section';
    newSectionDetailInfo.id = 'product-edit__section-detail'

    const sectionDienThoaiToAdd = `
    <div class="product-detail-panel product-specification-new">
        <div class="panel-header">
            <div class="panel-title"> Thông tin chi tiết </div>
        </div>
        <div class="panel-content-wrapper">
            <div class="panel-content">
                <div class="attribute-select-container-new">
                    <div class="attribute-select-list">
                        <div class="attribute-select-list-column">
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> Thương hiệu</span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <select class="tafi-select" name="">
                                                                <option value="" class="tafi-select-categories__option">Chọn thương hiệu</option>
                                                                <option value="" class="tafi-select-categories__option">Apple</option>
                                                                <option value="" class="tafi-select-categories__option">SAMSUNG</option>
                                                                <option value="" class="tafi-select-categories__option">OPPO</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> Màn hình </span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <input type="text" placeholder="Nhập vào" size="large"
                                                                   resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                   restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> Độ phân giải </span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <input type="text" placeholder="Nhập vào" size="large"
                                                                   resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                   restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> Camera </span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <input type="text" placeholder="Nhập vào" size="large"
                                                                   resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                   restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> Hệ điều hành </span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <input type="text" placeholder="Nhập vào" size="large"
                                                                   resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                   restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> Chip xử lý </span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <input type="text" placeholder="Nhập vào" size="large"
                                                                   resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                   restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="attribute-select-list-column">
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> ROM </span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <input type="text" placeholder="Nhập vào" size="large"
                                                                   resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                   restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> RAM </span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <input type="text" placeholder="Nhập vào" size="large"
                                                                   resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                   restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> Mạng di động </span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <input type="text" placeholder="Nhập vào" size="large"
                                                                   resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                   restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> Số khe sim </span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <input type="text" placeholder="Nhập vào" size="large"
                                                                   resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                   restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> Dung lượng pin </span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <input type="text" placeholder="Nhập vào" size="large"
                                                                   resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                   restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="attribute-select-item">
                                <div class="edit-row">
                                    <div class="edit-label edit-title">
                                        <span><span style="color: red;">*</span> Màu sắc </span>
                                    </div>
                                    <div class="degrade-wrap edit-row-right-medium">
                                        <div class="custom-len-calc-input product-edit-form-item">
                                            <div class="product-edit-form-item-content">
                                                <div class="popover-wrap">
                                                    <div class="tafi-input price-input product-edit-input">
                                                        <div class="tafi-input__inner tafi-input__inner--large">
                                                            <select class="tafi-select" name="">
                                                                <option value="" class="tafi-select-categories__option">Chọn màu</option>
                                                                <option value="" class="tafi-select-categories__option">Màu đỏ</option>
                                                                <option value="" class="tafi-select-categories__option">Màu vàng</option>
                                                                <option value="" class="tafi-select-categories__option">Màu đen</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
`;
    newSectionDetailInfo.innerHTML = sectionDienThoaiToAdd;
    // Nếu chi tiết đã hiển thị, thì xóa nó trước khi thêm lại
    if (isDetailDienThoaiVisible) {
        removeDetailDienThoai();
    }

    sectionBasicInfo.parentNode.insertBefore(newSectionDetailInfo, sectionBasicInfo.nextSibling);

    // Đặt lại trạng thái chi tiết
    isDetailDienThoaiVisible = true;

}

// Hàm để xóa bỏ chi tiết Điện thoại
function removeDetailDienThoai() {
    const sectionDetailInfo = document.getElementById('product-edit__section-detail');
    if (sectionDetailInfo) {
        sectionDetailInfo.parentNode.removeChild(sectionDetailInfo);
    }
    isDetailDienThoaiVisible = false;
}



//////////////////////////////////////////////////////////////////
// Dành cho chi tiết thòi trang                                //
////////////////////////////////////////////////////////////////

// Khai báo một biến để lưu trạng thái chi tiết
let isDetailThoiTrangVisible = false;

function detailThoiTrang() {

    const newSectionDetailInfo = document.createElement('section')
    newSectionDetailInfo.className = 'product-edit__section';
    newSectionDetailInfo.id = 'product-edit__section-detail'

    const sectionThoiTrangToAdd = `
        <div class="product-detail-panel product-specification-new">
            <div class="panel-header">
                <div class="panel-title"> Thông tin chi tiết </div>
            </div>
            <div class="panel-content-wrapper">
                <div class="panel-content">
                    <div class="attribute-select-container-new">
                        <div class="attribute-select-list">
                            <div class="attribute-select-list-column">
                                <div class="attribute-select-item">
                                    <div class="edit-row">
                                        <div class="edit-label edit-title">
                                            <span><span style="color: red;">*</span> Thương hiệu</span>
                                        </div>
                                        <div class="degrade-wrap edit-row-right-medium">
                                            <div class="custom-len-calc-input product-edit-form-item">
                                                <div class="product-edit-form-item-content">
                                                    <div class="popover-wrap">
                                                        <div class="tafi-input price-input product-edit-input">
                                                            <div class="tafi-input__inner tafi-input__inner--large">
                                                                <select class="tafi-select" name="">
                                                                    <option value="" class="tafi-select-categories__option">Chọn thương hiệu</option>
                                                                    <option value="" class="tafi-select-categories__option">Apple</option>
                                                                    <option value="" class="tafi-select-categories__option">SAMSUNG</option>
                                                                    <option value="" class="tafi-select-categories__option">OPPO</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="attribute-select-item">
                                    <div class="edit-row">
                                        <div class="edit-label edit-title">
                                            <span><span style="color: red;">*</span> Chất liệu </span>
                                        </div>
                                        <div class="degrade-wrap edit-row-right-medium">
                                            <div class="custom-len-calc-input product-edit-form-item">
                                                <div class="product-edit-form-item-content">
                                                    <div class="popover-wrap">
                                                        <div class="tafi-input price-input product-edit-input">
                                                            <div class="tafi-input__inner tafi-input__inner--large">
                                                                <input type="text" placeholder="Nhập vào" size="large"
                                                                       resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                       restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="attribute-select-item">
                                    <div class="edit-row">
                                        <div class="edit-label edit-title">
                                            <span><span style="color: red;">*</span> Size </span>
                                        </div>
                                        <div class="degrade-wrap edit-row-right-medium">
                                            <div class="custom-len-calc-input product-edit-form-item">
                                                <div class="product-edit-form-item-content">
                                                    <div class="popover-wrap">
                                                        <div class="tafi-input price-input product-edit-input">
                                                            <div class="tafi-input__inner tafi-input__inner--large">
                                                                <input type="text" placeholder="Nhập vào" size="large"
                                                                       resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                       restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="attribute-select-item">
                                    <div class="edit-row">
                                        <div class="edit-label edit-title">
                                            <span><span style="color: red;">*</span> Màu sắc </span>
                                        </div>
                                        <div class="degrade-wrap edit-row-right-medium">
                                            <div class="custom-len-calc-input product-edit-form-item">
                                                <div class="product-edit-form-item-content">
                                                    <div class="popover-wrap">
                                                        <div class="tafi-input price-input product-edit-input">
                                                            <div class="tafi-input__inner tafi-input__inner--large">
                                                                <select class="tafi-select" name="">
                                                                    <option value="" class="tafi-select-categories__option">Chọn màu</option>
                                                                    <option value="" class="tafi-select-categories__option">Màu đỏ</option>
                                                                    <option value="" class="tafi-select-categories__option">Màu vàng</option>
                                                                    <option value="" class="tafi-select-categories__option">Màu đen</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;
    newSectionDetailInfo.innerHTML = sectionThoiTrangToAdd;
    // Nếu chi tiết đã hiển thị, thì xóa nó trước khi thêm lại
    if (isDetailThoiTrangVisible) {
        removeDetailThoiTrang();
    }

    sectionBasicInfo.parentNode.insertBefore(newSectionDetailInfo, sectionBasicInfo.nextSibling);

    // Đặt lại trạng thái chi tiết
    isDetailThoiTrangVisible = true;

}

// Hàm để xóa bỏ chi tiết thời trang
function removeDetailThoiTrang() {
    const sectionDetailInfo = document.getElementById('product-edit__section-detail');
    if (sectionDetailInfo) {
        sectionDetailInfo.parentNode.removeChild(sectionDetailInfo);
    }
    isDetailThoiTrangVisible = false;
}



//////////////////////////////////////////////////////////////////
// Dành cho chi tiết giày                                      //
////////////////////////////////////////////////////////////////

// Khai báo một biến để lưu trạng thái chi tiết
let isDetailGiayVisible = false;

function detailGiay() {

    const newSectionDetailInfo = document.createElement('section')
    newSectionDetailInfo.className = 'product-edit__section';
    newSectionDetailInfo.id = 'product-edit__section-detail'

    const sectionGiayToAdd = `
        <div class="product-detail-panel product-specification-new">
            <div class="panel-header">
                <div class="panel-title"> Thông tin chi tiết </div>
            </div>
            <div class="panel-content-wrapper">
                <div class="panel-content">
                    <div class="attribute-select-container-new">
                        <div class="attribute-select-list">
                            <div class="attribute-select-list-column">
                                <div class="attribute-select-item">
                                    <div class="edit-row">
                                        <div class="edit-label edit-title">
                                            <span><span style="color: red;">*</span> Thương hiệu</span>
                                        </div>
                                        <div class="degrade-wrap edit-row-right-medium">
                                            <div class="custom-len-calc-input product-edit-form-item">
                                                <div class="product-edit-form-item-content">
                                                    <div class="popover-wrap">
                                                        <div class="tafi-input price-input product-edit-input">
                                                            <div class="tafi-input__inner tafi-input__inner--large">
                                                                <select class="tafi-select" name="">
                                                                    <option value="" class="tafi-select-categories__option">Chọn thương hiệu</option>
                                                                    <option value="" class="tafi-select-categories__option">Apple</option>
                                                                    <option value="" class="tafi-select-categories__option">SAMSUNG</option>
                                                                    <option value="" class="tafi-select-categories__option">OPPO</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="attribute-select-item">
                                    <div class="edit-row">
                                        <div class="edit-label edit-title">
                                            <span><span style="color: red;">*</span> Size giày </span>
                                        </div>
                                        <div class="degrade-wrap edit-row-right-medium">
                                            <div class="custom-len-calc-input product-edit-form-item">
                                                <div class="product-edit-form-item-content">
                                                    <div class="popover-wrap">
                                                        <div class="tafi-input price-input product-edit-input">
                                                            <div class="tafi-input__inner tafi-input__inner--large">
                                                                <input type="text" placeholder="Nhập vào" size="large"
                                                                       resize="none" rows="2" minrows="2" maxlength="Infinity"
                                                                       restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;
    newSectionDetailInfo.innerHTML = sectionGiayToAdd;
    // Nếu chi tiết đã hiển thị, thì xóa nó trước khi thêm lại
    if (isDetailGiayVisible) {
        removeDetailGiay();
    }

    sectionBasicInfo.parentNode.insertBefore(newSectionDetailInfo, sectionBasicInfo.nextSibling);

    // Đặt lại trạng thái chi tiết
    isDetailGiayVisible = true;

}

// Hàm để xóa bỏ chi tiết giày
function removeDetailGiay() {
    const sectionDetailInfo = document.getElementById('product-edit__section-detail');
    if (sectionDetailInfo) {
        sectionDetailInfo.parentNode.removeChild(sectionDetailInfo);
    }
    isDetailGiayVisible = false;
}



const htmlToAdd = `
    <div class="panel-content">
         <div class="edit-row">
             <div class="edit-row-left edit-label">
                 <span><span style="color: red;">*</span> Giá</span>
             </div>
             <div class="degrade-wrap edit-row-right-medium">
                 <div class="custom-len-calc-input product-edit-form-item">
                     <div class="product-edit-form-item-content">
                         <div class="popover-wrap">
                             <div class="tafi-input price-input product-edit-input">
                                 <div class="tafi-input__inner tafi-input__inner--large">
                                     <div class="tafi-input__prefix">
                                         đ
                                         <span class="tafi-input__prefix-split"></span>
                                     </div>
                                     <input type="text" placeholder="Nhập vào" size="large"
                                            resize="none" rows="2" minrows="2" maxlength="Infinity"
                                            restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                 </div>
                             </div>
                         </div>
                     </div>
                 </div>
             </div>
         </div>
         <div class="edit-row">
             <div class="edit-row-left edit-label">
                 <span><span style="color: red;">*</span> Số lượng tồn</span>
             </div>
             <div class="degrade-wrap edit-row-right-medium">
                 <div class="custom-len-calc-input product-edit-form-item">
                     <div class="product-edit-form-item-content">
                         <div class="popover-wrap">
                             <div class="tafi-input price-input product-edit-input">
                                 <div class="tafi-input__inner tafi-input__inner--large">
                                     <input type="text" placeholder="Nhập vào" size="large"
                                            resize="none" rows="2" minrows="2" maxlength="Infinity"
                                            restrictiontype="input" max="Infinity" min="-Infinity" class="tafi-input__input" id="text-input">
                                 </div>
                             </div>
                         </div>
                     </div>
                 </div>
             </div>
         </div>
     </div>
`;


selectCategories.addEventListener('change', function () {
    // Ẩn tất cả các bảng chi tiết

    // Lấy giá trị được chọn
    const selectedValue = selectCategories.value;

    // Hiển thị bảng chi tiết tương ứng nếu có giá trị được chọn
    if (selectedValue === 'dienthoai' || selectedValue === 'thoitrang' || selectedValue === 'giay') {
        removeElements();
        panelContentWrapper.innerHTML = htmlToAdd;
    }
    if (selectedValue === 'dienthoai') {
        removeDetailThoiTrang(); // Xóa bỏ chi tiết Thời trang
        removeDetailGiay(); // Xóa bỏ chi tiết Giày
        detailDienThoai();
    }
    if (selectedValue === 'thoitrang') {
        removeDetailDienThoai(); // Xóa bỏ chi tiết Điện thoại
        removeDetailGiay(); // Xóa bỏ chi tiết Giày
        detailThoiTrang();
    }
    if (selectedValue === 'giay') {
        removeDetailThoiTrang(); // Xóa bỏ chi tiết Thời trang
        removeDetailDienThoai(); // Xóa bỏ chi tiết Điện thoại
        detailGiay();
    }
    if (selectedValue === '') {
        removeDetailDienThoai(); // Xóa bỏ chi tiết Điện thoại
        removeDetailThoiTrang(); // Xóa bỏ chi tiết Thời trang
        removeDetailGiay(); // Xóa bỏ chi tiết Giày
        addElements();
        panelContentWrapper.innerHTML = `<div class="panel-inactive-content"> Có thể điều chỉnh sau khi chọn ngành hàng </div>`;

    }
});