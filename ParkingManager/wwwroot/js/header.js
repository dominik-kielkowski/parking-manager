document.addEventListener("DOMContentLoaded", async () => {
  const usernameSpan = document.getElementById("username");
  const authBtn = document.getElementById("authBtn");
  const navMenu = document.getElementById("navMenu");
  const adminBtn = document.getElementById("adminBtn");

  if (!usernameSpan || !authBtn) return;

  try {
    const res = await fetch("/auth/current", { credentials: "include" });
    const data = await res.json();

    if (data.isAuthenticated) {
      usernameSpan.textContent = `Hello, ${data.userName}`;
      authBtn.textContent = "Logout";
      authBtn.onclick = async () => {
        await fetch("/auth/logout", { method: "POST", credentials: "include" });
        window.location.reload();
      };
      navMenu.style.display = "flex";
      if (data.isAdmin) {
        adminBtn.style.display = "inline-block";
      }
      showScreen("spots");
    } else {
      usernameSpan.textContent = "";
      authBtn.textContent = "Login with GitHub";
      authBtn.onclick = () => {
        window.location.href = "/auth/login";
      };
      navMenu.style.display = "none";
    }
  } catch (err) {
    console.error("Failed to load current user:", err);
  }
});
