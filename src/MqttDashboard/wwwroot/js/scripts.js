window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        //Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }
// Toggle the right side navigation
    const rightSidebarToggle = document.body.querySelector('#rightSidebarToggle');
    if (rightSidebarToggle) {
        // Uncomment Below to persist right sidebar toggle between refreshes
        // if (localStorage.getItem('sb|right-sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-right-sidenav-toggled');
        // }
        rightSidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-right-sidenav-toggled');
            localStorage.setItem('sb|right-sidebar-toggle', document.body.classList.contains('sb-right-sidenav-toggled'));
        });
    }
});
