const labels = document.querySelectorAll('.form-control label')
labels.forEach(label => {
    label.innerHTML = label.innerText
        .split('')
        .map((letter, idx) => `<span style="transition-delay:${idx * 50}ms">${letter}</span>`)
        .join('')
})

//convert FormData to Object
var serializeFormToObject = function (form) {
    var objForm = {};
    var formData = new FormData(form);
    for (var key of formData.keys()) {
        objForm[key] = formData.get(key);
    }
    return objForm;
};

const loginform = document.querySelector('.container form')
// Submit Login Form 
loginform.addEventListener("submit", function (e) {
    e.preventDefault();
    fetch('/Users/Login', {
        method: 'post',
        body: JSON.stringify(serializeFormToObject(loginform)),
        headers: {
            'Content-type': 'application/json; charset=UTF-8'
        }
    }).then(function (response) {
        if (response.ok) {
            return response.json();
        }
        return Promise.reject(response);
    }).then(function (result) {
        if (result.err == "0")// Login Successfully
            // Redirect To TodoList Index Page
            window.location.replace("/Home/Index");
        else {// Login Failed
            alert(result.errMsg);
            // Refresh Current Login Index Page
            window.location.reload();
        }
    });
});