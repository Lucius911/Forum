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
                        <v-divider class="my-2"></v-divider>

                        <v-card-actions>
                            <v-btn icon>
                                <v-icon @click="toggleLike(post.id)" color="blue">mdi-thumb-up-outline</v-icon>
                                <span class="ml-1">{{ post.likesCount || 0 }}</span>
                            </v-btn>

                            <v-btn icon>
                                <v-icon @click="openCommentDialog(post.id)"
                                    color="grey darken-1">mdi-comment-outline</v-icon>
                                <span class="ml-1">{{ post.commentsCount || 0 }}</span>
                            </v-btn>
                        </v-card-actions>
                        <v-card-actions>
                            <div v-if="post.comments && post.comments.length">
                                <div v-for="comment in post.comments" :key="comment.id" class="pl-4 pb-2">
                                    Comment by: <strong>{{ comment.username || 'Anonymous' }}:</strong> <br />
                                    Message: {{ comment.content }}
                                </div>
                            </div>
                            <v-alert v-else type="info" dense text>
                                No comments yet. Be the first to comment!
                            </v-alert>
                        </v-card-actions>
                    </v-card>
                </v-col>
            </v-row>
        </section>
    </v-container>

    <!-- MODAL FOR COMMENT  -->
    <v-dialog v-model="commentDialog" max-width="500px">
        <v-card>
            <v-card-title class="headline">Add Comment</v-card-title>
            <v-card-text>
                <v-textarea v-model="newComment" label="Your Comment" rows="4" auto-grow outlined />
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn text @click="commentDialog = false">Cancel</v-btn>
                <v-btn color="primary" @click="submitComment">Submit</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>

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
            commentDialog: false,
            newComment: "",
            activePostId: null,
        };
    },
    methods: {
        async fetchPosts() {
            try {
                const res = await fetch("https://localhost:7189/api/forum/fetchall");
                if (!res.ok) throw new Error("Failed to fetch posts");
                this.posts = await res.json();
                console.log(this.posts);
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

            await this.fetchPosts();

            if (!result.ok) {
                return;
            }
        },
        openCommentDialog(postId) {
            this.activePostId = postId;
            this.commentDialog = true;
        },
        async submitComment() {
            //replace with toast notification when you have time
            if (!this.newComment.trim()) {
                alert("I don't think you want to submit an empty comment.");
                return;
            }

            console.log(this.newComment);
            console.log(this.activePostId);

            const result = await fetch("https://localhost:7189/api/forum/CreateComment", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("jwtToken")}`,
                },
                body: JSON.stringify({
                    forumPostId: this.activePostId,
                    comment: this.newComment
                }),
            });

            if (!result.ok) {
                alert("Failed to submit comment.");
                return;
            }

            this.commentDialog = false;
            await this.fetchPosts();

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
