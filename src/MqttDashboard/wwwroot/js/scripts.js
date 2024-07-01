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

// $('form').submit(function(event) {
//     // Prevent the default form submission
//     event.preventDefault();
//
//     // Serialize the form data
//     var formData = $(this).serialize();
//
//     // Submit the form using AJAX
//     $.ajax({
//         type: 'POST',
//         url: '/LogRequest/Index', // Adjust the URL if needed
//         data: formData,
//         success: function(response) {
//             // Handle success if needed
//             console.log(response);
//         },
//         error: function(xhr, status, error) {
//             // Handle errors if any
//             console.error(xhr.responseText);
//         }
//     });
// });


// Fetch messages every 1 seconds
// setInterval(fetchMessages, 1000);
//
// // Fetch messages immediately on load
// fetchMessages();
//
// function fetchMessages() {
//     $.ajax({
//         url: '@Url.Action("GetMessages")',
//         method: 'GET',
//         success: function(data) {
//             // Update the json-preview div with the new messages
//             $('#json-preview').text(JSON.stringify(data, null, 2));
//         },
//         error: function(xhr, status, error) {
//             console.error(xhr.responseText);
//         }
//     });
// }