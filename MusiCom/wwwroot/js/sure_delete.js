function sureSend(e) {
    e.preventDefault();

    Swal.fire({
        title: 'Are you sure?',
        text: "Do you really want to delete this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then(result => {
        if (result.isConfirmed) {
            const myForm = document.getElementById('myForm');
            myForm.submit();
        }
    })
}