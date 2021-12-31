function active_nav(id) {
    document.querySelectorAll('.nav-item a').forEach(x => x.classList.remove('active'));
    document.querySelector('#' + id + ' a').classList.add('active');
}

function createSpinnerEl() {
    var containerDiv = Object.assign(document.createElement("div"), { id: 'spinner-div' });
    containerDiv.classList.add('container');
    var spinner = document.createElement("div");
    spinner.classList.add('loading');
    spinner.id = 'loading';
    containerDiv.appendChild(spinner);
    spinner.style.visibility = 'visible';
    return containerDiv;
}