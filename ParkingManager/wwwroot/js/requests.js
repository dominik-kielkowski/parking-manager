document.addEventListener('DOMContentLoaded', () => {
    const requestForm = document.getElementById('requestForm');

    if (requestForm) {
        requestForm.onsubmit = async (e) => {
            e.preventDefault();
            const spotNumber = document.getElementById('spotNumber').value;
            const reason = document.getElementById('reason').value;

            try {
                const res = await fetch('/requests/send', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ spotNumber, reason })
                });
                const text = await res.text();
                console.log(text);
                alert('Request sent successfully!');
                requestForm.reset();
            } catch (err) {
                console.error('Error sending request:', err);
                alert('Failed to send request.');
            }
        };
    }
});