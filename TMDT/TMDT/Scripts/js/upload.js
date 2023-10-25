//const imageInput = document.getElementById('image-input');
//const previewContainer = document.querySelector('.preview-container');
//const imageCountText = document.getElementById('image-count'); // Lấy thẻ p hiển thị số đếm

//let imageCount = 0; // Biến đếm số lượng hình ảnh đã hiển thị

//imageInput.addEventListener('change', () => {
//    const files = imageInput.files;

//    for (let i = 0; i < files.length; i++) {
//        if (imageCount >= 3) {
//            break; // Đã hiển thị 3 hình ảnh, thoát khỏi vòng lặp
//        }
//        const file = files[i];
//        if (file.type.startsWith('image/')) {
//            const reader = new FileReader();
//            reader.readAsDataURL(file);

//            reader.onload = () => {
//                const img = document.createElement('img');
//                img.src = reader.result;
//                img.className = 'preview-image-product';

//                // Thêm thuộc tính 'name' vào thẻ img

//                previewContainer.appendChild(img);

//                imageCount++; // Tăng biến đếm sau khi thêm hình ảnh

//                updateImageCountText(); // Cập nhật số đếm
//            };
//            //const reader = new FileReader();
//            //reader.readAsDataURL(file);

//            //reader.onload = () => {
//            //    const img = document.createElement('img');
//            //    img.src = reader.result;
//            //    img.className = 'preview-image-product';

//            //    // Thêm thuộc tính 'name' vào thẻ img
//            //    if (imageCount === 0) {
//            //        img.setAttribute('name', 'UploadImage1');
//            //    } else if (imageCount === 1) {
//            //        img.setAttribute('name', 'UploadImage2');
//            //    } else if (imageCount === 2) {
//            //        img.setAttribute('name', 'UploadImage3');
//            //    }

//            //    previewContainer.appendChild(img);

//            //    imageCount++; // Tăng biến đếm sau khi thêm hình ảnh

//            //    updateImageCountText(); // Cập nhật số đếm
//            //};
//        }
//    }
//});

//// Hàm cập nhật số đếm trong thẻ p
//function updateImageCountText() {
//    imageCountText.textContent = imageCount;
//}
//function ChangeImage(UploadImage, previewImg1, previewImg2, previewImg3) {
//    if (UploadImage.files && UploadImage.files[0]) {
//        var reader = new FileReader();
//        reader.onload = function (e) {
//            $(previewImg1).attr('src', e.target.result);
//            $(previewImg2).attr('src', e.target.result);
//            $(previewImg3).attr('src', e.target.result);
//        }
//        reader.readAsDataURL(UploadImage.files[0]);
//    }
//}


function ChangeImage(UploadImage, previewImg1) {
    if (UploadImage.files && UploadImage.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImg1).attr('src', e.target.result);
        }
        reader.readAsDataURL(UploadImage.files[0]);
    }
}

function ChangeImage2(UploadImage2, previewImg2) {
    if (UploadImage2.files && UploadImage2.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImg2).attr('src', e.target.result);
        }
        reader.readAsDataURL(UploadImage2.files[0]);
    }
}
function ChangeImage3(UploadImage3, previewImg3) {
    if (UploadImage3.files && UploadImage3.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImg3).attr('src', e.target.result);
        }
        reader.readAsDataURL(UploadImage3.files[0]);
    }
}


//function ChangeImages(inputImages, previewImages) {
//    for (let i = 0; i < inputImages.length; i++) {
//        if (inputImages[i].files && inputImages[i].files[0]) {
//            var reader = new FileReader();
//            reader.onload = function (e) {
//                $(previewImages[i]).attr('src', e.target.result);
//            }
//            reader.readAsDataURL(inputImages[i].files[0]);
//        }
//    }
//}

// đếm ký tự nhập vào ô input có giới hạn là 120
const textInput = document.getElementById('text-input');
const charCount = document.getElementById('char-count');
const inputChar = document.getElementsByClassName('tafi-input__char-count');

textInput.addEventListener('input', function () {
    const inputText = textInput.value;
    const numberOfCharacters = inputText.length;

    // Kiểm tra số ký tự đã nhập và giới hạn nó không vượt quá 120
    if (numberOfCharacters > 120) {
        textInput.value = inputText.substring(0, 120); // Cắt bỏ ký tự thừa

        // Kiểm tra nếu số ký tự bằng 120 thì thêm class "warning"
        for (let i = 0; i < inputChar.length; i++) {
            inputChar[i].classList.add('warning');
        }
    } else {
        for (let i = 0; i < inputChar.length; i++) {
            inputChar[i].classList.remove('warning');
        }
    }

    charCount.textContent = textInput.value.length;

    
});

// đếm ký tự nhập vào ô input có giới hạn là 1000
const txtAreaInput = document.getElementById('tafi-input-area');
const txtAreaCount = document.getElementById('text-area-pre');
const inputTxtArea = document.getElementsByClassName('text-area-label');

txtAreaInput.addEventListener('input', function () {
    const inputText = txtAreaInput.value;
    const numberOfCharacters = inputText.length;

    // Kiểm tra số ký tự đã nhập và giới hạn nó không vượt quá 1000
    if (numberOfCharacters > 1000) {
        txtAreaInput.value = inputText.substring(0, 1000); // Cắt bỏ ký tự thừa

        // Kiểm tra nếu số ký tự bằng 1000 thì thêm class "warning"
        for (let i = 0; i < inputTxtArea.length; i++) {
            inputTxtArea[i].classList.add('warning');
        }
    } else {
        for (let i = 0; i < inputTxtArea.length; i++) {
            inputTxtArea[i].classList.remove('warning');
        }
    }

    txtAreaCount.textContent = txtAreaInput.value.length;


});




//Ngăn chặn việc tải lại trang khi có thay đổi dữ liệu
//window.addEventListener("beforeunload", function (e) {
//    // Hiển thị thông báo xác nhận trước khi tải lại trang
//    const confirmationMessage = "Bạn có muốn tải lại trang không?";
//    e.returnValue = confirmationMessage; // Thông báo sẽ được hiển thị trong hộp thoại xác nhận trình duyệt
//    return confirmationMessage; // Thông báo sẽ được hiển thị trong một số trình duyệt cũ
//});