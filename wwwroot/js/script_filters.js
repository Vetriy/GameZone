const checkboxButtons = [
    { element: document.getElementById("checkbox_1"), key: "sport", active: true },
    { element: document.getElementById("checkbox_2"), key: "race", active: true },
    { element: document.getElementById("checkbox_3"), key: "shoot", active: true },
    { element: document.getElementById("checkbox_4"), key: "automat", active: true }
];

checkboxButtons.forEach(item => {
    item.element.addEventListener('click', function () {
        if (item.active) {
            if (checkboxButtons.some(otherItem => otherItem.active && otherItem !== item)) {
                item.element.classList.remove('selected');
                item.active = false;
            }
        }
        else {
            item.element.classList.add('selected');
            item.active = true;
        }
    });
});

const radioButtons = [
    { element: document.getElementById("radio_1"), key: "namegame", active: true },
    { element: document.getElementById("radio_2"), key: "cost", active: false },
    { element: document.getElementById("radio_3"), key: "date", active: false },
    { element: document.getElementById("radio_4"), key: "rate", active: false }
];

radioButtons.forEach(item => {
    item.element.addEventListener('click', function () {

        radioButtons.forEach(elem => {
            if (elem.key != item.key) {
                elem.element.classList.remove('selected');
                elem.active = false;
            }
        });

        if (!item.active) {
            item.element.classList.add('selected');
            item.active = true;
        }
    });
});

document.getElementById("increasing_sorting").addEventListener('click', function () {
    document.getElementById("increasing_sorting").classList.add('selected');
    document.getElementById("decreasing_sorting").classList.remove('selected');
});

document.getElementById("decreasing_sorting").addEventListener('click', function () {
    document.getElementById("decreasing_sorting").classList.add('selected');
    document.getElementById("increasing_sorting").classList.remove('selected');
});