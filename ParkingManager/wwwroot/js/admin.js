document.addEventListener('DOMContentLoaded', async () => {
    const tableBody = document.querySelector('#requestsTable tbody');

    if (!tableBody) return;

    async function loadRequests() {
        try {
            const res = await fetch('/admin/requests');
            const requests = await res.json();
            tableBody.innerHTML = '';

            requests.forEach(req => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${req.id}</td>
                    <td>${req.userName}</td>
                    <td>${req.spotNumber}</td>
                    <td>${req.reason}</td>
                    <td>
                        <button onclick="handleAction(${req.id}, 'approve')">Approve</button>
                        <button onclick="handleAction(${req.id}, 'reject')">Reject</button>
                    </td>
                `;
                tableBody.appendChild(row);
            });
        } catch (err) {
            console.error('Error loading requests:', err);
        }
    }

    window.handleAction = async (id, action) => {
        try {
            const res = await fetch(`/admin/${action}?id=${id}`, { method: 'POST' });
            const text = await res.text();
            console.log(text);
            loadRequests(); // Refresh table
        } catch (err) {
            console.error(`Error ${action}ing request:`, err);
        }
    };

    loadRequests();
});