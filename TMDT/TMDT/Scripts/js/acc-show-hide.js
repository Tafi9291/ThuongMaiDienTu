const info = document.querySelectorAll('.info');
const buttonEditAcc = document.getElementById('button-edit-1');
const buttonEditAcc2 = document.getElementById('button-edit-2');
const buttonEditAcc3 = document.getElementById('button-edit-3');
const buttonEditAcc4 = document.getElementById('button-edit-4');
const extraContent = document.querySelectorAll('.extra');
const desc = document.getElementById('desc');
const desc2 = document.getElementById('desc-2');
const desc3 = document.getElementById('desc-3');
const desc4 = document.getElementById('desc-4');

console.log(desc)

//if (buttonEditAcc) {
//    buttonEditAcc.addEventListener('click', () => {
//        if (buttonEditAcc.innerHTML === 'Sửa') {
//            buttonEditAcc.innerHTML = 'Hủy';
//            info[0].style.display = "none";
//            extraContent[0].style.display = "block";
//            if (desc.classList.contains('width-info')) {
//                desc.classList.remove('width-info');
//            }
//        } else {
//            buttonEditAcc.innerHTML = 'Sửa';
//            info[0].style.display = "block";
//            extraContent[0].style.display = "none";
//            desc.classList.add('width-info');
//        }
//    });
//}

//if (buttonEditAcc2) {
//    buttonEditAcc2.addEventListener('click', () => {
//        if (buttonEditAcc2.innerHTML === 'Sửa') {
//            buttonEditAcc2.innerHTML = 'Hủy';
//            info[1].style.display = "none";
//            extraContent[1].style.display = "block";
//            if (desc2.classList.contains('width-info')) {
//                desc2.classList.remove('width-info');
//            }
//        } else {
//            buttonEditAcc2.innerHTML = 'Sửa';
//            info[1].style.display = "block";
//            extraContent[1].style.display = "none";
//            desc2.classList.add('width-info');
//        }
//    });
//}

function handleEditClick(button, infoElement, extraContentElement, descElement) {
    if (button.innerHTML === 'Sửa') {
        button.innerHTML = 'Hủy';
        infoElement.style.display = "none";
        extraContentElement.style.display = "block";
        if (descElement.classList.contains('width-info')) {
            descElement.classList.remove('width-info');
        }
    } else {
        button.innerHTML = 'Sửa';
        infoElement.style.display = "block";
        extraContentElement.style.display = "none";
        descElement.classList.add('width-info');
    }
}

if (buttonEditAcc) {
    buttonEditAcc.addEventListener('click', () => {
        handleEditClick(buttonEditAcc, info[0], extraContent[0], desc);
    });
}

if (buttonEditAcc2) {
    buttonEditAcc2.addEventListener('click', () => {
        handleEditClick(buttonEditAcc2, info[1], extraContent[1], desc2);
    });
}

if (buttonEditAcc3) {
    buttonEditAcc3.addEventListener('click', () => {
        handleEditClick(buttonEditAcc2, info[2], extraContent[2], desc3);
    });
}

if (buttonEditAcc4) {
    buttonEditAcc4.addEventListener('click', () => {
        handleEditClick(buttonEditAcc2, info[3], extraContent[3], desc4);
    });
}
