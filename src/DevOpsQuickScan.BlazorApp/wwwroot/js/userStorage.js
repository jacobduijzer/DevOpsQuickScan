window.userStorage = {
    getOrCreateUserId: function () {
        const key = "pollingAppUserId";
        let userId = localStorage.getItem(key);
        if (!userId) {
            userId = crypto.randomUUID();
            localStorage.setItem(key, userId);
        }
        return userId;
    }
};