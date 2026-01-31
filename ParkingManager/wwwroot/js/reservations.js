document.addEventListener('DOMContentLoaded', async () => {
    const tableBody = document.querySelector('#reservationsTable tbody');
    if (!tableBody) return;

    async function loadReservations() {
        try {
            const res = await fetch('/parking/reservations');

            if (res.status === 401) {
                tableBody.innerHTML = '';
                return;
            }

            if (!res.ok) {
                console.error('Failed to load reservations:', res.status);
                tableBody.innerHTML = '<tr><td colspan="5">Error loading reservations</td></tr>';
                return;
            }

            const reservations = await res.json();
            tableBody.innerHTML = '';

            if (!reservations.length) {
                tableBody.innerHTML = '<tr><td colspan="5">No reservations</td></tr>';
                return;
            }

            reservations.forEach(resv => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${resv.id}</td>
                    <td>${resv.userName}</td>
                    <td>${resv.spotNumber}</td>
                    <td>${new Date(resv.date).toLocaleString()}</td>
                    <td>${resv.status}</td>
                `;
                tableBody.appendChild(row);
            });
        } catch (err) {
            console.error('Error loading reservations:', err);
            tableBody.innerHTML = '<tr><td colspan="5">Error loading reservations</td></tr>';
        }
    }

    loadReservations();
});