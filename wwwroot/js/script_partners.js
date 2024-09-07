const partners = [
    { logo: 'content/дельфин.png', name: 'ООО "Дельфинчик"' },
    { logo: 'content/вискас.png', name: 'ООО "Здоровая еда"' },
    { logo: 'content/кирка.png', name: 'OOO "Minecraft"' },
    { logo: 'content/спанчбоб.png', name: 'Наше воображееение' }
];

let currentIndexPartner = 0;

function updatePartner() {
    document.getElementById('partner_logo').src = partners[currentIndexPartner].logo;
    document.getElementById('partner_name').textContent = partners[currentIndexPartner].name;
}

document.getElementById('partners_left_arrow').addEventListener('click', () => {
    currentIndexPartner = (currentIndexPartner - 1 + partners.length) % partners.length;
    updatePartner();
});

document.getElementById('partners_right_arrow').addEventListener('click', () => {
    currentIndexPartner = (currentIndexPartner + 1) % partners.length;
    updatePartner();
});