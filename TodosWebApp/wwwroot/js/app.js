class Todo {
    /** 带有 JSON转换作用 的Todo类
     * Todo 对象 在 数据库 与 前端 中 属性名与属性数据类型存在差异, 
     * 所以从后台获取额JSON与前台使用的JSON对象属性存在区别,
     * 在 isCompleted(frontend) 与 todos_isCompleted(backend) JSON属性对比可见差异性
     * @param {Array(t_todos)} data
     */
    constructor(data) {
        this.todos_identity = data.todos_identity;
        this.todos_textContent = data.todos_textContent;
        this.todos_isCompleted = data.todos_isCompleted;
    }
    get identity() { return this.todos_identity; }
    get textContent() { return this.todos_textContent; }
    set textContent(value) { this.todos_textContent = value; }
    get isCompleted() { return this.todos_isCompleted ? true : false; }
    set isCompleted(value) { value == true ? this.todos_isCompleted = 1 : this.todos_isCompleted = 0; }
}

let filtor = {
    all: function (todo) {
        return true;
    },
    active: function (todo) {
        return todo.isCompleted == false;
    },
    completed: function (todo) {
        return todo.isCompleted;
    }
}

let backend = (function () {
    let todosRestfulapi = function () {
    }
    todosRestfulapi.prototype.get = function () {
        // GET request for remote todos
        return axios({
            method: 'get',
            url: '/Home/GetAll',
            responseType: 'json'
        });
    }
    todosRestfulapi.prototype.save = function (todos) {
        // Post request for remote todos: Update, Insert, Delete
        return axios({
            method: 'post',
            url: '/Home/Save',
            data: {
                todos: todos
            }
        });
    }

    return new todosRestfulapi();

})();

let app = new Vue({
    el: ".wrapper",
    created() {
        backend
            .get()
            .then(function (response) {
                app.todos = response.data
                    .map(todo => new Todo(todo));
            });
    },
    data() {
        return {
            todos: [],
            filterType: window.location.hash.replace(/#\/?/, '') || "all",
            newTodo: '',
            editedTodo: null,
        }
    },
    watch: {
        todos: {
            handler: function (todos) {
                backend.save(todos);
            },
            deep: true
        }
    },
    methods: {
        addTodo: function () {
            var value = this.newTodo && this.newTodo.trim()
            if (!value) {
                return;
            }
            this.todos.push(new Todo({
                todos_identity: (_.max(this.todos.map(t => t.identity)) || 0) + 1 ,// 取最大identity+1为新的unique identity, 没有todo时为1
                todos_textContent: value,
                todos_isCompleted: 0
            }));
            this.newTodo = ''
        },
        deleteTodo: function (identity) {
            this.todos = this.todos.filter(todo => todo.identity != identity);
        },
        editTodo: function (todo) {
            this.beforeEditTodoCache = todo.textContent;
            this.editedTodo = todo;
        },
        doneEdit: function (todo) {
            this.editedTodo = null;
            if (!todo.textContent) {
                this.deleteTodo(todo.identity);
            }
        },
        cancelEdit: function (todo) {
            todo.textContent = this.beforeEditTodoCache;
            this.editedTodo = null;
        },
        clearCompletedTodos: function () {
            this.todos = this.todos.filter(todo => filtor["active"](todo));
        }
    },
    computed: {
        filteredTodos: function () {
            return this.todos.filter(todo => filtor[this.filterType](todo));
        },
        remaining: function () {
            return this.todos.filter(todo => filtor["active"](todo)).length;
        },
        allDone: {
            get: function () {
                return this.remaining === 0;
            },
            set: function (value) {
                this.todos.forEach(todo => todo.isCompleted = value);
            }
        }
    },
    directives: {
        "todo-focus": function (el, binding) {
            if (binding.value) {
                el.focus();
            }
        }
    }
});

window.addEventListener('hashchange', onHashChange);
// 处理路由
function onHashChange() {
    let hash = window.location.hash.replace(/#\/?/, '');
    if (filtor[hash]) {
        app.filterType = hash;
    } else {
        window.location.hash = '';
        app.filterType = 'all';
    }
}

