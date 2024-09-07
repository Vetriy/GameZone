let isNew = true;
let idForChange = 0;

document.getElementById('add_slot_machine_button').addEventListener('click', function () {
    document.getElementById('dark_background').style.display = 'block';
    document.getElementById('add_slote_machine').style.display = 'block';
    document.body.style.overflow = 'hidden';

    document.getElementById('add_slote_machine_header').querySelector('p').textContent = "Добавить новый игровой автомат";
    document.getElementById('add_slote_machine_name').value = "";
    document.getElementById('add_slote_machine_type').value = "Sport";
    document.getElementById('add_slote_machine_description').value = "";
    document.getElementById('add_slote_machine_cost').value = "";
    document.getElementById('add_slote_machine_date').value = "";
    document.getElementById('add_slote_machine_rate').value = 7;
    document.getElementById('add_slote_machine_rate_value').textContent = 7;

    isNew = true;
});

document.addEventListener('changeSlotMachine', function (event) {
    document.getElementById('dark_background').style.display = 'block';
    document.getElementById('add_slote_machine').style.display = 'block';
    document.body.style.overflow = 'hidden';

    isNew = false;
    idForChange = event.detail.id;

    fetch(`/Home/GetInfoSlotMachine?id=${idForChange}`)
        .then(response => response.json())
        .then(data => {
            document.getElementById('add_slote_machine_header').querySelector('p').textContent = "Редактировать игровой автомат";
            document.getElementById('add_slote_machine_name').value = data[0];
            document.getElementById('add_slote_machine_type').value = data[1];
            document.getElementById('add_slote_machine_description').value = data[2];
            document.getElementById('add_slote_machine_cost').value = data[3];
            document.getElementById('add_slote_machine_date').value = data[4];
            document.getElementById('add_slote_machine_rate').value = data[5];
            document.getElementById('add_slote_machine_rate_value').textContent = data[5];
        });

});

document.getElementById('close_add_slote_machine_button').addEventListener('click', function () {
    document.getElementById('dark_background').style.display = 'none';
    document.getElementById('add_slote_machine').style.display = 'none';
    document.body.style.overflow = 'auto';

    setErrorNone();
});

document.getElementById('add_slote_machine_rate').addEventListener('input', function () {
    document.getElementById('add_slote_machine_rate_value').textContent = add_slote_machine_rate.value;
});

document.getElementById('add_slote_machine_button').addEventListener('click', function () {

    let name = document.getElementById('add_slote_machine_name').value;
    let type = document.getElementById('add_slote_machine_type').value;
    let description = document.getElementById('add_slote_machine_description').value;
    let cost = document.getElementById('add_slote_machine_cost').value;
    let date = document.getElementById('add_slote_machine_date').value;
    let rate = document.getElementById('add_slote_machine_rate').value;

    const creationDate = new Date("1995-03-06");
    const thisDate = new Date(date);
    const todayDate = new Date();

    setErrorNone();
    let is_error = false;

    if (name == '') {
        document.getElementById('error_empty_name').style.display = 'block';
        is_error = true;
    }

    if (description == '') {
        document.getElementById('error_empty_description').style.display = 'block';
        is_error = true;
    }

    if (cost == '') {
        document.getElementById('error_empty_cost').style.display = 'block';
        is_error = true;
    }

    if (date == '') {
        document.getElementById('error_empty_date').style.display = 'block';
        is_error = true;
    }

    if (thisDate.getTime() < creationDate.getTime()) {
        document.getElementById('error_little_date').style.display = 'block';
        is_error = true;
    }

    if (thisDate.getTime() > todayDate.getTime()) {
        document.getElementById('error_big_date').style.display = 'block';
        is_error = true;
    }

    if (!is_error) {
        if (isNew) {
            fetch(`/Home/AddSlotMachineButton?name=${normalizeString(name)}&type=${type}&description=${description}&cost=${cost}&date=${date}&rate=${rate}`)
                .then(response => response.json())
                .then(data => {
                    if (data == -1) {
                        document.getElementById('error_repeated_name').style.display = 'block';
                    }
                    else {
                        document.getElementById('dark_background').style.display = 'none';
                        document.getElementById('add_slote_machine').style.display = 'none';
                        document.body.style.overflow = 'auto';

                        document.dispatchEvent(new Event('updateButtonEvent'));
                        document.dispatchEvent(new Event('slotMachineUpdate'));
                    }
                });
        }
        else {
            fetch(`/Home/ChangeSlotMachineButton?id=${idForChange}&name=${normalizeString(name)}&type=${type}&description=${description}&cost=${cost}&date=${date}&rate=${rate}`)
                .then(response => response.json())
                .then(data => {
                    if (data == -1) {
                        document.getElementById('error_repeated_name').style.display = 'block';
                    }
                    else {
                        document.getElementById('dark_background').style.display = 'none';
                        document.getElementById('add_slote_machine').style.display = 'none';
                        document.body.style.overflow = 'auto';

                        document.dispatchEvent(new Event('updateButtonEvent'));
                        document.dispatchEvent(new Event('slotMachineUpdate'));
                    }
                });
        }
    }
});

function normalizeString(inputString) {
    let trimmedString = inputString.trim();
    let normalizedString = trimmedString.replace(/\s+/g, ' ');
    return normalizedString;
}

function setErrorNone() {
    document.getElementById('error_empty_name').style.display = 'none';
    document.getElementById('error_empty_description').style.display = 'none';
    document.getElementById('error_empty_cost').style.display = 'none';
    document.getElementById('error_empty_date').style.display = 'none';
    document.getElementById('error_little_date').style.display = 'none';
    document.getElementById('error_big_date').style.display = 'none';
    document.getElementById('error_repeated_name').style.display = 'none';
}