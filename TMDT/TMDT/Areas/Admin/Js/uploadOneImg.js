const imageInputBrand = document.getElementById('image-input-brand');
const previewContainer = document.querySelector('.preview-container');
const imageCountText2 = document.getElementById('image-count-2'); // Lấy thẻ p hiển thị số đếm

let imageCount = 0; // Biến đếm số lượng hình ảnh đã hiển thị


imageInputBrand.addEventListener('change', () => {
    const files = imageInputBrand.files;

    for (let i = 0; i < files.length; i++) {
        if (imageCount >= 1) {
            break; // Đã hiển thị 1 hình ảnh, thoát khỏi vòng lặp
        }
        const file = files[i];
        if (file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.readAsDataURL(file);

            reader.onload = () => {
                const img = document.createElement('img');
                img.src = reader.result;
                img.className = 'preview-image-product';

                // Thêm thuộc tính 'name' vào thẻ img
                if (imageCount === 0) {
                    img.setAttribute('name', 'image1');
                }

                previewContainer.appendChild(img);

                imageCount++; // Tăng biến đếm sau khi thêm hình ảnh

                updateImageCountText2(); // Cập nhật số đếm
            };
        }
    }
});

// Hàm cập nhật số đếm trong thẻ p
function updateImageCountText2() {
    imageCountText2.textContent = imageCount;
}

//Ngăn chặn việc tải lại trang khi có thay đổi dữ liệu
window.addEventListener("beforeunload", function (e) {
    // Hiển thị thông báo xác nhận trước khi tải lại trang
    const confirmationMessage = "Bạn có muốn tải lại trang không?";
    e.returnValue = confirmationMessage; // Thông báo sẽ được hiển thị trong hộp thoại xác nhận trình duyệt
    return confirmationMessage; // Thông báo sẽ được hiển thị trong một số trình duyệt cũ
});