var searchKeyword = document.querySelector('input[name="search"]').value;

// Tạo một đối tượng XMLHttpRequest mới
var xhr = new XMLHttpRequest();
xhr.open('GET', 'search.php?search=' + encodeURIComponent(searchKeyword), true);

// Xử lý kết quả trả về từ máy chủ
xhr.onload = function () {
    if (xhr.status === 200) {
        // Cập nhật nội dung của div có id là 'search-results' với kết quả trả về
        document.getElementById('search-results').innerHTML = xhr.responseText;
    } else {
        // Xử lý lỗi (nếu có)
        console.error('Lỗi khi tìm kiếm: ' + xhr.statusText);
    }
};

// Gửi yêu cầu
xhr.send();
});