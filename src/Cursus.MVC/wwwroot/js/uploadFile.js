// document.getElementById('uploadArea').addClickListener('click', () => {
//     document.getElementById('fileInput').click();
// });

document.getElementById('fileInput').addEventListener('change', (event) => {
    const fileList = document.getElementById('fileList');
    fileList.innerHTML = '';

    Array.from(event.target.files).forEach(file => {
        const reader = new FileReader();
        reader.onload = (e) => {
            const fileItem = document.createElement('div');
            fileItem.classList.add('file-item');
            fileItem.classList.add('d-flex');
            fileItem.classList.add('justify-content-between');
            fileItem.classList.add('align-items-center');

            const img = document.createElement('img');
            img.src = e.target.result;

            const deleteBtn = document.createElement('button');
            deleteBtn.classList.add('delete-btn');
            deleteBtn.classList.add('border-0');
            deleteBtn.innerHTML = '<i class="fas fa-trash-alt"></i>';
            deleteBtn.addEventListener('click', () => {
                fileItem.remove();
            });

            fileItem.appendChild(img);
            fileItem.appendChild(deleteBtn);
            fileList.appendChild(fileItem);
        };
        reader.readAsDataURL(file);
    });
});
