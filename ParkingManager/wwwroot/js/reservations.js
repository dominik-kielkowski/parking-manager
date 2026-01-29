document.addEventListener('DOMContentLoaded', async () => {
    const tableBody = document.querySelector('#reservationsTable tbody');
    if (!tableBody) return;

    async function loadReservations() {
        try {
            const res = await fetch('/parking/reservations');
            const reservations = await res.json();

            tableBody.innerHTML = '';

            reservations.forEach(r => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${r.id}</td>
                    <td>${r.userName}</td>
                    <td>${r.spotNumber}</td>
                    <td>${new Date(r.date).toLocaleString()}</td>
                    <td>${r.status}</td>
                `;
                tableBody.appendChild(row);
            });
        } catch (err) {
            console.error('Error loading reservations:', err);
        }
    }

    loadReservations();
});
