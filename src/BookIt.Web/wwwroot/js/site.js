// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// ─── Dark Mode ───
(function () {
    var saved = localStorage.getItem('bookit-theme') || 'light';
    document.documentElement.setAttribute('data-theme', saved);
})();

function toggleDarkMode() {
    var current = document.documentElement.getAttribute('data-theme') || 'light';
    var next = current === 'dark' ? 'light' : 'dark';
    document.documentElement.setAttribute('data-theme', next);
    localStorage.setItem('bookit-theme', next);
    updateDarkModeIcons();
}

function updateDarkModeIcons() {
    var isDark = document.documentElement.getAttribute('data-theme') === 'dark';
    document.querySelectorAll('.dark-mode-icon').forEach(function (el) {
        el.className = 'dark-mode-icon bi ' + (isDark ? 'bi-sun-fill' : 'bi-moon-fill');
    });
}

document.addEventListener('DOMContentLoaded', function () {
    updateDarkModeIcons();
});
