document.addEventListener('DOMContentLoaded', async () => {
    const usernameSpan = document.getElementById('username');
    const authBtn = document.getElementById('authBtn');

    if (!usernameSpan || !authBtn) return;

    try {
        const res = await fetch('/auth/current', { credentials: 'include' });
        const data = await res.json();

        console.log('Current user data:', data); // check in console

        if (data.isAuthenticated) {
            usernameSpan.textContent = `Hello, ${data.userName}`;
            authBtn.textContent = 'Logout';
            authBtn.onclick = async () => {
                await fetch('/auth/logout', { method: 'POST', credentials: 'include' });
                window.location.reload();
            };
        } else {
            usernameSpan.textContent = '';
            authBtn.textContent = 'Login with GitHub';
            authBtn.onclick = () => {
                window.location.href = '/auth/login';
            };
        }
    } catch (err) {
        console.error('Failed to load current user:', err);
    }
});
