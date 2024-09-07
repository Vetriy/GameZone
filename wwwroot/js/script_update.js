UpdateButton();

document.getElementById("update_button").addEventListener("click", function () {
    UpdateButton();
});
document.getElementById("reset_all_button").addEventListener("click", function () {
    document.getElementById("reset_all_button").style.display = 'none';
    fetch(`/Home/ResetAll`)
        .then(response => response.json())
        .then(data => {
            UpdateButton();
        });
});


function deleteClick(element) {

    let id = element.parentElement.parentElement.querySelector('.slot_machine').querySelector('.slot_machine_id').innerText;
    fetch(`/Home/DeleteSlotMachineButton?id=${id}`)
        .then(response => response.json())
        .then(data => {
            UpdateButton();
        });
}

function changeClick(element) {
    let id_ = element.parentElement.parentElement.querySelector('.slot_machine').querySelector('.slot_machine_id').innerText;
    document.dispatchEvent(new CustomEvent('changeSlotMachine', { detail: { id: id_ } }));
}

document.addEventListener('updateButtonEvent', function () {
    UpdateButton();
});

function UpdateButton() {
    let gameGenre = [];
    checkboxButtons.forEach(elem => {
        if (elem.active) {
            gameGenre.push(elem.key);
        }
    });

    let newSortingType = '';
    radioButtons.forEach(elem => {
        if (elem.active) {
            newSortingType = elem.key;
        }
    });

    let isAscending = document.getElementById("decreasing_sorting").classList.contains('selected');

    fetch(`/Home/GameUpdateButton?game_genre=${gameGenre}&new_sorting_type=${newSortingType}&i_or_d=${isAscending}`)
        .then(response => response.json())
        .then(data => {
            var slotContainers = document.getElementById("slot_machines");
            slotContainers.innerHTML = "";

            data.forEach(container => {

                var title = document.createElement("p");
                title.textContent = container[0];

                var description = document.createElement("label");
                description.innerHTML = container[1].replace(/\n/g, '<br>');

                var index = document.createElement("p");
                index.textContent = container[2];
                index.style.display = 'none';
                index.classList.add("slot_machine_id");

                var containerDiv = document.createElement("div");
                containerDiv.classList.add("slot_machine");
                containerDiv.appendChild(title);
                containerDiv.appendChild(description);
                containerDiv.appendChild(index);

                var deleteContainer = document.createElement("div");
                deleteContainer.classList.add("slot_machine_delete_container");
                deleteContainer.setAttribute("onclick", "deleteClick(this)");
                deleteContainer.appendChild(deleteIcon());

                var changeContainer = document.createElement("div");
                changeContainer.classList.add("slot_machine_change_container");
                changeContainer.setAttribute("onclick", "changeClick(this)");
                changeContainer.appendChild(changeIcon());

                var icons = document.createElement("div");
                icons.classList.add("slot_machine_edit");
                icons.appendChild(deleteContainer);
                icons.appendChild(changeContainer);

                var container = document.createElement("div");
                container.classList.add("slot_machine_container");
                container.appendChild(containerDiv);
                container.appendChild(icons);

                slotContainers.appendChild(container);
            });

            if (data.length <= 0) {
                document.getElementById('reset_all_button').style.display = 'block';
            }
            else {
                document.getElementById('reset_all_button').style.display = 'none';
            }

            document.dispatchEvent(new Event('slotMachineUpdate'));
        });
}

function deleteIcon() {

    const svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
    svg.setAttribute("enable-background", "new 0 0 32 32");
    svg.setAttribute("viewBox", "0 0 32 32");
    svg.setAttribute("xmlns", "http://www.w3.org/2000/svg");
    svg.classList.add("slot_machine_svg");
    svg.classList.add("slot_machine_delete");
    const path = document.createElementNS("http://www.w3.org/2000/svg", "path");
    path.setAttribute("d", "m23.164 32.021h-14.319c-1.654 0-3-1.346-3-3v-19.8c0-.553.448-1 1-1s1 .447 1 1v19.801c0 .552.449 1 1 1h14.319c.552 0 1-.448 1-1v-19.801c0-.553.447-1 1-1s1 .447 1 1v19.801c0 1.654-1.346 2.999-3 2.999zm-2.764-4.396c-.553 0-1-.447-1-1v-17.404c0-.553.447-1 1-1s1 .447 1 1v17.404c0 .553-.447 1-1 1zm-4.396 0c-.552 0-1-.447-1-1v-17.404c0-.553.448-1 1-1s1 .447 1 1v17.404c0 .553-.448 1-1 1zm-4.397 0c-.552 0-1-.447-1-1v-17.404c0-.553.448-1 1-1s1 .447 1 1v17.404c0 .553-.447 1-1 1zm16.397-21.344h-24.091c-.552 0-1-.447-1-1s.448-1 1-1h5.02c.14-2.024.971-2.661 2.029-3.259.994-.562 2.766-1.001 4.033-1.001h2.019c1.268 0 3.039.439 4.032 1 1.059.599 1.89 1.236 2.029 3.26h4.929c.553 0 1 .447 1 1s-.447 1-1 1zm-17.069-2h10.138c-.081-.992-.394-1.169-1.011-1.519-.699-.395-2.124-.741-3.049-.741h-2.019c-.924 0-2.349.347-3.049.742-.617.349-.928.526-1.01 1.518z");
    svg.appendChild(path);

    return svg;
}

function changeIcon() {

    const svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
    svg.setAttribute("xmlns", "http://www.w3.org/2000/svg");
    svg.setAttribute("xmlns:xlink", "http://www.w3.org/1999/xlink");
    svg.setAttribute("x", "0");
    svg.setAttribute("y", "0");
    svg.setAttribute("viewBox", "0 0 469.333 469.333");
    svg.setAttribute("style", "enable-background:new 0 0 469.333 469.333;");
    svg.setAttribute("xml:space", "preserve");
    svg.classList.add("slot_machine_svg");
    svg.classList.add("slot_machine_change");

    const group1 = document.createElementNS("http://www.w3.org/2000/svg", "g");
    const rect = document.createElementNS("http://www.w3.org/2000/svg", "rect");
    rect.setAttribute("x", "21.333");
    rect.setAttribute("y", "426.667");
    rect.setAttribute("width", "426.667");
    rect.setAttribute("height", "42.667");
    group1.appendChild(rect);
    svg.appendChild(group1);

    const group2 = document.createElementNS("http://www.w3.org/2000/svg", "g");
    const path = document.createElementNS("http://www.w3.org/2000/svg", "path");
    path.setAttribute("d", "M327.253,0L64,263.253V384h120.747L448,120.747L327.253,0z M167.04,341.333h-60.373V280.96L327.253,60.373l60.373,60.373L167.04,341.333z");
    group2.appendChild(path);
    svg.appendChild(group2);

    return svg;
}