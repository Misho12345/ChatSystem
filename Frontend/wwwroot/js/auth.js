const Auth = (function () {
    let accessToken = null;
    let refreshToken = null;

    function storeTokens(access, refresh) {
        accessToken = access;
        refreshToken = refresh;
        localStorage.setItem('accessToken', access);
        localStorage.setItem('refreshToken', refresh);
    }

    function loadTokens() {
        accessToken = localStorage.getItem('accessToken');
        refreshToken = localStorage.getItem('refreshToken');
    }

    function clearTokens() {
        accessToken = null;
        refreshToken = null;
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
    }

    async function login(identifier, password) {
        try {
            const response = await Api.login({tagOrEmail: identifier, password});
            storeTokens(response.accessToken, response.refreshToken);
            window.location.href = 'main.html';
        } catch (error) {
            console.error('Login failed:', error);
            displayError(error.message || 'Login failed. Please check your credentials.');
        }
    }

    async function register(name, tag, email, password) {
        try {
            await Api.register({name, tag, email, password});
            alert('Registration successful! Please login.');
            document.getElementById('registerForm').classList.add('d-none');
            document.getElementById('loginForm').classList.remove('d-none');
            document.getElementById('loginIdentifier').value = email;
        } catch (error) {
            console.error('Registration failed:', error);
            displayError(error.message || 'Registration failed. Please try again.');
        }
    }

    async function attemptRefresh() {
        if (!refreshToken) return false;
        try {
            const response = await Api.refreshToken({refreshToken});
            storeTokens(response.accessToken, response.refreshToken);
            console.log('Token refreshed successfully.');
            return true;
        } catch (error) {
            console.error('Token refresh failed:', error);
            clearTokens();
            window.location.href = 'index.html';
            return false;
        }
    }

    function displayError(message) {
        const errorDiv = document.getElementById('errorMessage');
        if (errorDiv) {
            errorDiv.textContent = message;
            errorDiv.classList.remove('d-none');
        }
    }

    function hideError() {
        const errorDiv = document.getElementById('errorMessage');
        if (errorDiv) {
            errorDiv.classList.add('d-none');
        }
    }

    function init() {
        loadTokens();
        if (window.location.pathname.endsWith('main.html') && !Auth.getAccessToken()) {
            window.location.href = 'index.html';
            return;
        }


        const loginForm = document.getElementById('loginForm');
        if (loginForm) {
            loginForm.addEventListener('submit', (e) => {
                e.preventDefault();
                hideError();
                const identifier = document.getElementById('loginIdentifier').value;
                const pass = document.getElementById('loginPassword').value;
                login(identifier, pass);
            });
        }

        const registerForm = document.getElementById('registerForm');
        if (registerForm) {
            registerForm.addEventListener('submit', (e) => {
                e.preventDefault();
                hideError();
                const name = document.getElementById('registerName').value;
                const tag = document.getElementById('registerTag').value;
                const email = document.getElementById('registerEmail').value;
                const pass = document.getElementById('registerPassword').value;
                register(name, tag, email, pass);
            });
        }
    }

    return {
        init,
        login,
        register,
        logout: function () {
            if (refreshToken) {
                Api.logout({refreshToken}).catch(err => console.error("Logout API call failed", err));
            }
            clearTokens();
            window.location.href = 'index.html';
        },
        getAccessToken: () => accessToken,
        getRefreshToken: () => refreshToken,
        attemptRefresh,
        isLoggedIn: () => !!accessToken
    };
})();