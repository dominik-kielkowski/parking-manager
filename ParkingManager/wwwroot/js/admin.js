document.addEventListener('DOMContentLoaded', async () => {
    const tableBody = document.querySelector('#requestsTable tbody');
    if (!tableBody) return;

    const ADMIN_ID = 3;

    async function loadRequests() {
        const res = await fetch('/AccessRequests');
        const requests = await res.json();

        tableBody.innerHTML = '';

        requests.forEach(req => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${req.id}</td>
                <td>${req.userName}</td>
                <td>${req.type}</td>
                <td>${req.isApproved}</td>
                <td>
                    <button onclick="handleAction(${req.id}, true)">Approve</button>
                    <button onclick="handleAction(${req.id}, false)">Reject</button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    }

    window.handleAction = async (requestId, approve) => {
        await fetch('/AccessRequests/review', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                requestId,
                approve,
                adminId: ADMIN_ID
            })
        });

        loadRequests();
    };

    loadRequests();
});
