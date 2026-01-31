document.addEventListener('DOMContentLoaded', () => {
    const requestForm = document.getElementById('requestForm');
    if (!requestForm) return;

    requestForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        const requestedAccess = document.getElementById('requestedAccess').value;

        try {
            const res = await fetch('/AccessRequests', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ requestedAccess })
            });

            if (!res.ok) {
                const text = await res.text();
                console.error('Error sending request:', text);
                alert('Failed to send request');
                return;
            }

            const data = await res.json();
            console.log('Access request created:', data);
            alert('Request sent successfully!');
            requestForm.reset();

            location.reload();
        } catch (err) {
            console.error('Network error:', err);
            alert('Failed to send request');
        }
    });
});
