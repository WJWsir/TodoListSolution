const labels = document.querySelectorAll('.form-control label')
const loginform = document.querySelectorAll('.container form')
labels.forEach(label => {
    label.innerHTML = label.innerText
        .split('')
        .map((letter, idx) => `<span style="transition-delay:${idx * 50}ms">${letter}</span>`)
        .join('')
})


loginform[0].addEventListener("submit", function (e) {
    e.preventDefault();
    axios({
        method: 'post',
        url: '/Users/Login',
        data: {
            t_users: 
        },
        responseType: 'json'
    })
        .then(function (response) {
            alert(response);
        });
});