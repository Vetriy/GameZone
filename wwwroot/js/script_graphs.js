let slotMachines = document.querySelectorAll('.slot_machine');
let date_list = [];
let stepY = 100000;
let createDate = new Date();
let id;

function setClickListenerForSlotMachines() {
    slotMachines.forEach(function (element) {
        element.addEventListener('click', function () {
            document.getElementById('dark_background').style.display = 'block';
            document.getElementById('statistic').style.display = 'block';
            document.body.style.overflow = 'hidden';

            document.getElementById('error_left_date').style.display = 'none';
            document.getElementById('error_right_date').style.display = 'none';

            setTodayData();

            id = parseInt(element.querySelector('.slot_machine_id').textContent);
            console.log(parseInt(element.querySelector('.slot_machine_id').textContent));
            fetch(`/Home/GraphsUpdateButton?id=${id}&dates=${date_list}`)
                .then(response => response.json())
                .then(data => {
                    drawGraph(data);
                    let parts = data[7].split('.');
                    createDate = new Date(parseInt(parts[2]), parseInt(parts[1]) - 1, parseInt(parts[0]), 3);
                });
        });
    });
}

document.addEventListener('slotMachineUpdate', function () {
    slotMachines = document.querySelectorAll('.slot_machine');
    setClickListenerForSlotMachines();
});

setClickListenerForSlotMachines();

document.getElementById('close_statistics_button').addEventListener('click', function () {

    if (document.getElementById('edit_date').style.display == 'block') return;

    document.getElementById('dark_background').style.display = 'none';
    document.getElementById('statistic').style.display = 'none';
    document.body.style.overflow = 'auto';
});

document.getElementById('statistics_left_arrow').addEventListener('click', function () {
    const thisDate = new Date(document.getElementById('statistic_ui_time').value);

    const newDate = new Date(thisDate);
    newDate.setDate(thisDate.getDate() - 7);

    setData(newDate);

    fetch(`/Home/GraphsUpdateButton?id=${id}&dates=${date_list}`)
        .then(response => response.json())
        .then(data => {
            drawGraph(data);
        });
});

document.getElementById('statistics_right_arrow').addEventListener('click', function () {
    const thisDate = new Date(document.getElementById('statistic_ui_time').value);

    const newDate = new Date(thisDate);
    newDate.setDate(thisDate.getDate() + 7);

    setData(newDate);

    fetch(`/Home/GraphsUpdateButton?id=${id}&dates=${date_list}`)
        .then(response => response.json())
        .then(data => {
            drawGraph(data);
        });
});

document.getElementById('statistic_ui_update_button').addEventListener('click', function () {
    const newDate = new Date(document.getElementById('statistic_ui_time').value);
    setData(newDate);

    fetch(`/Home/GraphsUpdateButton?id=${id}&dates=${date_list}`)
        .then(response => response.json())
        .then(data => {
            drawGraph(data);
        });
});

window.addEventListener("wheel", function (event) {
    if (document.getElementById('statistic').contains(event.target)) {
        let numbersCount = stepY.toString().length;

        if (event.deltaY < 0) {
            if (stepY > 1000) {
                if (stepY == Math.pow(10, numbersCount - 1)) {
                    stepY -= Math.pow(10, numbersCount - 2);
                }
                else {
                    stepY -= Math.pow(10, numbersCount - 1);
                }
            }
        } else {
            if (stepY < 10000000) {
                stepY += Math.pow(10, numbersCount - 1);
            }
        }

        document.getElementById('statistic_canvas_value_1').textContent = (stepY * 5).toString();
        document.getElementById('statistic_canvas_value_2').textContent = (stepY * 4).toString();
        document.getElementById('statistic_canvas_value_3').textContent = (stepY * 3).toString();
        document.getElementById('statistic_canvas_value_4').textContent = (stepY * 2).toString();
        document.getElementById('statistic_canvas_value_5').textContent = stepY.toString();

        const thisDate = new Date(document.getElementById('statistic_ui_time').value);
        setData(thisDate);

        fetch(`/Home/GraphsUpdateButton?id=${id}&dates=${date_list}`)
            .then(response => response.json())
            .then(data => {
                drawGraph(data);
            });
    }
});

function drawGraph(data) {

    const canvas = document.getElementById('statistic_canvas');

    canvas.width = 440;
    canvas.height = 200;

    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // Отрисовка координатных прямых

    ctx.beginPath();

    ctx.strokeStyle = 'rgba(0, 0, 0, 0.75)';
    ctx.lineWidth = 1;

    ctx.moveTo(20, 5);
    ctx.lineTo(20, 180);
    ctx.lineTo(435, 180);

    ctx.moveTo(20, 5);
    ctx.lineTo(17, 15);

    ctx.moveTo(20, 5);
    ctx.lineTo(23, 15);

    ctx.moveTo(435, 180);
    ctx.lineTo(425, 177);

    ctx.moveTo(435, 180);
    ctx.lineTo(425, 183);

    for (let i = 1; i <= 5; i++) {
        ctx.moveTo(17, 180 - 30 * i);
        ctx.lineTo(23, 180 - 30 * i);
    }

    for (let i = 1; i <= 7; i++) {
        ctx.moveTo(20 + 55 * i, 177);
        ctx.lineTo(20 + 55 * i, 183);
    }

    ctx.stroke();

    // Линии для оценки значений

    ctx.beginPath();

    ctx.strokeStyle = 'rgba(0, 0, 0, 0.05)';
    ctx.lineWidth = 1;

    for (let i = 1; i <= 5; i++) {
        ctx.moveTo(23, 180 - 30 * i);
        ctx.lineTo(435, 180 - 30 * i);
    }

    for (let i = 1; i <= 7; i++) {
        ctx.moveTo(20 + 55 * i, 177);
        ctx.lineTo(20 + 55 * i, 5);
    }

    ctx.stroke();

    // Отрисовка графика

    ctx.beginPath();

    ctx.strokeStyle = 'rgba(0, 0, 0, 1)';
    ctx.lineWidth = 1;

    const pixelsPerInit = 30 / stepY;

    ctx.moveTo(75, 180 - parseInt(data[0]) * pixelsPerInit);

    for (let i = 1; i < 7; i++) {
        ctx.lineTo(75 + 55 * i, 180 - parseInt(data[i]) * pixelsPerInit);
    }

    ctx.stroke();

    for (let i = 1; i <= 7; i++) {
        let id = "day_" + i;
        document.getElementById(id).title = data[i - 1].toString();
    }
}

function setData(currentDate) {

    document.getElementById('error_left_date').style.display = 'none';
    document.getElementById('error_right_date').style.display = 'none';

    let diff = currentDate.getDay();
    if (diff != 0) diff -= 4;
    else diff = 3;

    const today = new Date();
    if (currentDate.getTime() - (3 + diff) * 24 * 60 * 60 * 1000 > today.getTime()) {
        setTodayData();
        document.getElementById('error_left_date').style.display = 'none';
        document.getElementById('error_right_date').style.display = 'block';
        return;
    }

    if (currentDate.getTime() + (3 - diff) * 24 * 60 * 60 * 1000 < createDate.getTime()) {
        setData(createDate);
        document.getElementById('error_left_date').style.display = 'block';
        document.getElementById('error_right_date').style.display = 'none';
        return;
    }

    const thursdayDate = new Date(currentDate);
    thursdayDate.setDate(currentDate.getDate() - diff);

    date_list = [];

    for (let i = 1; i <= 7; i++) {
        let date = new Date(thursdayDate);
        date.setDate(date.getDate() + i - 4);
        date_list.push(date.toISOString().slice(0, 10));

        let id = "day_" + i;
        document.getElementById(id).textContent = date.getDate();
    }

    document.getElementById('statistic_ui_time').value = thursdayDate.toISOString().slice(0, 10);
}

function setTodayData() {
    const currentDate = new Date();
    setData(currentDate);
}

/* Edit */

let thisEditDate = '1995-03-06'

document.getElementById('day_1').addEventListener('click', function () {
    thisEditDate = date_list[0];
    DisplayEditDate();
});

document.getElementById('day_2').addEventListener('click', function () {
    thisEditDate = date_list[1];
    DisplayEditDate();
});

document.getElementById('day_3').addEventListener('click', function () {
    thisEditDate = date_list[2];
    DisplayEditDate();
});

document.getElementById('day_4').addEventListener('click', function () {
    thisEditDate = date_list[3];
    DisplayEditDate();
});

document.getElementById('day_5').addEventListener('click', function () {
    thisEditDate = date_list[4];
    DisplayEditDate();
});

document.getElementById('day_6').addEventListener('click', function () {
    thisEditDate = date_list[5];
    DisplayEditDate();
});

document.getElementById('day_7').addEventListener('click', function () {
    thisEditDate = date_list[6];
    DisplayEditDate();
});

function DisplayEditDate() {

    let thisDate = new Date(thisEditDate);
    let todayDate = new Date();

    if (thisDate.getTime() > todayDate.getTime() || thisDate.getTime() < createDate.getTime()) return;

    document.getElementById('edit_date').style.display = 'block';
    document.getElementById('error_empty_cost_date').style.display = 'none';
    document.getElementById('edit_date_header').querySelector('p').textContent = '';

    let valueRt = thisEditDate.split('-');

    document.getElementById('edit_date_header').querySelector('p').textContent += valueRt[2];
    document.getElementById('edit_date_header').querySelector('p').textContent += ' ';

    if (valueRt[1] == '01') document.getElementById('edit_date_header').querySelector('p').textContent += 'января';
    else if (valueRt[1] == '02') document.getElementById('edit_date_header').querySelector('p').textContent += 'февраля';
    else if (valueRt[1] == '03') document.getElementById('edit_date_header').querySelector('p').textContent += 'марта';
    else if (valueRt[1] == '04') document.getElementById('edit_date_header').querySelector('p').textContent += 'апреля';
    else if (valueRt[1] == '05') document.getElementById('edit_date_header').querySelector('p').textContent += 'мая';
    else if (valueRt[1] == '06') document.getElementById('edit_date_header').querySelector('p').textContent += 'июня';
    else if (valueRt[1] == '07') document.getElementById('edit_date_header').querySelector('p').textContent += 'июля';
    else if (valueRt[1] == '08') document.getElementById('edit_date_header').querySelector('p').textContent += 'августа';
    else if (valueRt[1] == '09') document.getElementById('edit_date_header').querySelector('p').textContent += 'сентября';
    else if (valueRt[1] == '10') document.getElementById('edit_date_header').querySelector('p').textContent += 'октября';
    else if (valueRt[1] == '11') document.getElementById('edit_date_header').querySelector('p').textContent += 'ноября';
    else if (valueRt[1] == '12') document.getElementById('edit_date_header').querySelector('p').textContent += 'декабря';

    document.getElementById('edit_date_header').querySelector('p').textContent += ' ';
    document.getElementById('edit_date_header').querySelector('p').textContent += valueRt[0];
    document.getElementById('edit_date_header').querySelector('p').textContent += ' года';


    fetch(`/Home/GetInfoDate?date=${thisEditDate}&id=${id}`)
        .then(response => response.json())
        .then(data => {
            document.getElementById('edit_date_cost').value = data;
        });
}

document.getElementById('close_edit_date_button').addEventListener('click', function () {
    document.getElementById('edit_date').style.display = 'none';
});

document.getElementById('edit_date_button').addEventListener('click', function () {

    if (document.getElementById('edit_date_cost').value == '') {
        document.getElementById('error_empty_cost_date').style.display = 'block';
    }
    else {
        fetch(`/Home/EditDate?date=${thisEditDate}&id=${id}&newValue=${document.getElementById('edit_date_cost').value}`)
            .then(response => response.json())
            .then(data => {
                document.getElementById('edit_date').style.display = 'none';

                fetch(`/Home/GraphsUpdateButton?id=${id}&dates=${date_list}`)
                    .then(response => response.json())
                    .then(data2 => {
                        console.log("hphoohpph");
                        drawGraph(data2);
                    });
            });

        
    }
});

