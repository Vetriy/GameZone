let creationDate = new Date("1995-03-06");

document.getElementById("show_diagrams_button").addEventListener('click', function () {
    document.getElementById('dark_background').style.display = 'block';
    document.getElementById('diagram').style.display = 'block';
    document.body.style.overflow = 'hidden';

    const today = new Date();
    let startDate = "1995-03-06";
    let endDate = today.toISOString().slice(0, 10);

    document.getElementById('diagram_ui_start_time').value = startDate;
    document.getElementById('diagram_ui_end_time').value = endDate;

    fetch(`/Home/ChartUpdateButton?date_start=${startDate}&date_end=${endDate}`)
        .then(response => response.json())
        .then(data => {
            drawChart(data);
        });
});

document.getElementById('close_diagrams_button').addEventListener('click', function () {
    document.getElementById('dark_background').style.display = 'none';
    document.getElementById('diagram').style.display = 'none';
    document.body.style.overflow = 'auto';
});

document.getElementById("diagram_ui_update_button").addEventListener('click', function () {

    let dateStartString = document.getElementById('diagram_ui_start_time').value;
    let dateEndString = document.getElementById('diagram_ui_end_time').value;

    const today = new Date();

    if (dateStartString == "") {
        dateStartString = creationDate.toISOString().slice(0, 10);
        document.getElementById('diagram_ui_start_time').value = dateStartString;
    }
    else {
        let dateStart = new Date(dateStartString);

        if (dateStart.getTime() < creationDate.getTime()) {
            dateStartString = creationDate.toISOString().slice(0, 10);
            document.getElementById('diagram_ui_start_time').value = dateStartString;
        }

        if (dateStart.getTime() > today.getTime()) {
            dateStartString = today.toISOString().slice(0, 10);
            document.getElementById('diagram_ui_start_time').value = dateStartString;
        }
    }

    if (dateEndString == "") {
        dateEndString = today.toISOString().slice(0, 10);
        document.getElementById('diagram_ui_end_time').value = dateEndString;
    }
    else {
        let dateEnd = new Date(dateEndString);

        if (dateEnd.getTime() > today.getTime()) {
            dateEndString = today.toISOString().slice(0, 10);
            document.getElementById('diagram_ui_end_time').value = dateEndString;
        }
    }

    let dateStart = new Date(dateStartString);
    let dateEnd = new Date(dateEndString);

    if (dateEnd.getTime() < dateStart.getTime()) {
        dateEndString = dateStart.toISOString().slice(0, 10);
        document.getElementById('diagram_ui_end_time').value = dateEndString;
    }

    fetch(`/Home/ChartUpdateButton?date_start=${dateStartString}&date_end=${dateEndString}`)
        .then(response => response.json())
        .then(data => {
            drawChart(data);
        });
});

function drawChart(data) {

    const canvas = document.getElementById('diagram_canvas');

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

    ctx.stroke();

    // Линии для оценки значений

    ctx.beginPath();

    ctx.strokeStyle = 'rgba(0, 0, 0, 0.05)';
    ctx.lineWidth = 1;

    for (let i = 1; i <= 5; i++) {
        ctx.moveTo(23, 180 - 30 * i);
        ctx.lineTo(435, 180 - 30 * i);
    }

    ctx.stroke();

    // Отрисовка столбцов

    const colors = [
        [0, 153, 102],
        [255, 102, 0],
        [102, 102, 255],
        [255, 204, 51]
    ];

    let max = 0;
    for (let i = 1; i <= 4; i++) {
        let sum = 0;
        data[i - 1].forEach(elem => { sum += parseInt(elem[1]); });
        max = Math.max(max, sum);

        let id = "diagram_type_" + i;
        document.getElementById(id).title = sum.toString();
    }

    let stepY = 1;
    while (stepY * 5 < max) {
        stepY += Math.pow(10, stepY.toString().length - 1);
    }

    document.getElementById('diagram_canvas_value_1').textContent = (stepY * 5).toString();
    document.getElementById('diagram_canvas_value_2').textContent = (stepY * 4).toString();
    document.getElementById('diagram_canvas_value_3').textContent = (stepY * 3).toString();
    document.getElementById('diagram_canvas_value_4').textContent = (stepY * 2).toString();
    document.getElementById('diagram_canvas_value_5').textContent = stepY.toString();
    max = stepY * 5;
    const pixelsPerInit = 150 / max;

    for (let i = 0; i < 4; i++) {

        let alpha_chanel = 1;
        let height = 0;
        let gameHeight = 0;
        let difference = 0.5 / (data[i].length - 1);

        for (let j = 0; j < data[i].length; j++) {
            ctx.beginPath();
            ctx.fillStyle = `rgba(${colors[i][0]}, ${colors[i][1]}, ${colors[i][2]}, ${alpha_chanel})`;

            height += parseInt(data[i][j][1]);
            gameHeight = parseInt(data[i][j][1]);

            ctx.rect(
                65 + 90 * i,
                180 - height * pixelsPerInit,
                40,
                gameHeight * pixelsPerInit
            );

            ctx.fill();
            alpha_chanel -= difference;
        }
    }

    creationDate = new Date(data[4][0][0]);
}