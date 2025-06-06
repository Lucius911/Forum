<template>
    <v-container class="py-10">
        <v-row justify="space-between" align="center">
            <v-col>
                <h1 class="text-h4 font-weight-bold">Welcome to Our Forum</h1>
            </v-col>
            <v-col class="text-end">
                <v-btn color="primary" @click="goToLogin" class="me-2">Login</v-btn>
                <v-btn color="secondary" @click="goToRegister">Register</v-btn>
            </v-col>
        </v-row>

        <v-divider class="my-6"></v-divider>

        <section class="posts">
            <h2 class="text-h5 mb-4">Latest Posts</h2>

            <div v-if="loading">
                <v-progress-circular indeterminate color="primary" />
            </div>

            <v-btn color="primary" @click="$router.push('/create-post')" class="mt-2" v-if="isAuthenticated">
                Create Post
            </v-btn>

            <v-alert v-if="!loading && !posts.length && !isAuthenticated" type="info" variant="outlined">
                No posts available. To create a post, please register or sign in.
            </v-alert>

            <v-alert v-else-if="error" type="error" variant="outlined">
                {{ error }}
            </v-alert>

            <v-row v-else>
                <v-col cols="12" md="6" v-for="post in posts" :key="post.id">
                    <v-card>
                        <v-card-title>{{ post.title }}</v-card-title>
                        <v-card-text>{{ post.content }}</v-card-text>

                        <v-card-actions>
                            <v-btn icon>
                                <v-icon @click="toggleLike(post.id)" color="blue">mdi-thumb-up-outline</v-icon>
                                <span class="ml-1">{{ post.likesCount || 0 }}</span>
                            </v-btn>

                            <v-btn icon>
                                <v-icon color="grey darken-1">mdi-comment-outline</v-icon>
                                <span class="ml-1">{{ post.commentsCount || 0 }}</span>
                            </v-btn>
                        </v-card-actions>
                    </v-card>
                </v-col>
            </v-row>
        </section>
    </v-container>
</template>

<script>
export default {
    name: "LandingPage",
    data() {
        return {
            posts: [],
            loading: true,
            error: null,
            isAuthenticated: false,
        };
    },
    methods: {
        async fetchPosts() {
            try {
                const res = await fetch("https://localhost:7189/api/forum/fetchall");
                if (!res.ok) throw new Error("Failed to fetch posts");
                this.posts = await res.json();
            } catch (err) {
                this.error = err.message;
            } finally {
                this.loading = false;
            }
        },
        goToLogin() {
            this.$router.push("/login");
        },
        goToRegister() {
            this.$router.push("/register");
        },
        async toggleLike(postId) {
            console.log(this.isAuthenticated);
            if (!this.isAuthenticated) {
               this.$router.push("/login");
            }

            const result = await fetch(`https://localhost:7189/api/forum/ToggleLike/${postId}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("jwtToken")}`,
                },
            });

            if (!result.ok) {
                return;
            }
        },
        isUserAuthenticated() {
            var tokenExists = localStorage.getItem("jwtToken");
            this.isAuthenticated = !!tokenExists;
        },
    },
    mounted() {
        this.fetchPosts();
        this.isUserAuthenticated();
    },
};
</script>

<style scoped>
.landing {
    padding: 20px;
}

.auth-buttons {
    margin-top: 10px;
}

button {
    margin-right: 10px;
}

.posts {
    margin-top: 30px;
}

.v-card-actions {
    display: flex;
    gap: 16px;
    align-items: center;
}

.ml-1 {
    margin-left: 4px;
}
</style>
