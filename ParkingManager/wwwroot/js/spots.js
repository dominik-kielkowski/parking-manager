document.addEventListener('DOMContentLoaded', () => {

    const spotsUrl = '/parking/spots';
    const bookUrl = '/parking/book?spotNumber=';

    async function loadSpots() {
        try {
            const response = await fetch(spotsUrl);
            const spots = await response.json();

            const grid = document.getElementById('spotsGrid');
            grid.innerHTML = '';

            spots.forEach(spot => {
                const btn = document.createElement('button');
                btn.textContent = spot.spotNumber;
                btn.className = 'spotButton';

                if (spot.isTaken) {
                    btn.classList.add('taken');
                    btn.disabled = true;
                }

                btn.onclick = async () => {
                    const res = await fetch(`${bookUrl}${spot.spotNumber}`, { method: 'POST' });
                    const text = await res.text();

                    btn.classList.add('taken');
                    btn.disabled = true;
                    btn.style.backgroundColor = 'grey';
                    btn.style.color = '#fff';

                    console.log(text);
                };

                grid.appendChild(btn);
            });

        } catch (err) {
            console.error('Error loading spots:', err);
        }
    }

    loadSpots();
});