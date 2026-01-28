document.addEventListener('DOMContentLoaded', async () => {
    const tableBody = document.querySelector('#reservationsTable tbody');

    if (!tableBody) return;

    async function loadReservations() {
        try {
            const res = await fetch('/reservations');
            const reservations = await res.json();
            tableBody.innerHTML = '';

            reservations.forEach(resv => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${resv.id}</td>
                    <td>${resv.userName}</td>
                    <td>${resv.spotNumber}</td>
                    <td>${resv.date}</td>
                    <td>${resv.status}</td>
                `;
                tableBody.appendChild(row);
            });
        } catch (err) {
            console.error('Error loading reservations:', err);
        }
    }

    loadReservations();
});