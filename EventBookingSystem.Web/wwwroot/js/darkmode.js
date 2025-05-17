document.addEventListener('DOMContentLoaded', () => {
    const themeSwitch = document.getElementById('theme-switch');
    const body = document.body;

    // Load saved theme from localStorage
    const savedTheme = localStorage.getItem('theme') || 'light';
    body.classList.add(savedTheme + '-theme');

    // Toggle theme on button click
    themeSwitch.addEventListener('click', () => {
        if (body.classList.contains('light-theme')) {
            body.classList.replace('light-theme', 'dark-theme');
            localStorage.setItem('theme', 'dark');
        } else {
            body.classList.replace('dark-theme', 'light-theme');
            localStorage.setItem('theme', 'light');
        }
    });
});