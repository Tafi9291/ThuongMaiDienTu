// Lấy tất cả các menu items
const menuItems = document.querySelectorAll('.sidebar-menu-box');

// Lặp qua từng menu item và thêm sự kiện click
menuItems.forEach((menuItem) => {
    const menuHeader = menuItem.querySelector('.sidebar-menu-item');
    const submenu = menuItem.querySelector('.sidebar-submenu');
    const menuToggle = menuHeader.querySelector('.sidebar-menu-item-collapse');

    menuHeader.addEventListener('click', () => {
        submenu.classList.toggle('active');
        menuToggle.classList.toggle('rotate');
    });
});





