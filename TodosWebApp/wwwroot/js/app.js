class Todo {
    /** 带有 JSON转换作用 的Todo类
     * Todo 对象 在 后端 与 前端 中 属性名与属性数据类型存在差异, 
     * 所以从后台获取额JSON与前台使用的JSON对象属性存在区别,
     * 在 isCompleted(frontend) 与 todo_isCompleted(backend) JSON属性对比可见差异性
     * @param {Array(t_todos)} data
     */
    constructor(data) {
        this.todo_identity = data.todo_identity;
        this.todo_textContent = data.todo_textContent;
        this.todo_isCompleted = data.todo_isCompleted;
    }
    get identity() { return this.todo_identity; }
    get textContent() { return this.todo_textContent; }
    set textContent(value) { this.todo_textContent = value; }
    get isCompleted() { return this.todo_isCompleted ? true : false; }
    set isCompleted(value) { value == true ? this.todo_isCompleted = 1 : this.todo_isCompleted = 0; }
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

//convert FormData to Object
var serializeFormToObject = function (form) {
    var objForm = {};
    var formData = new FormData(form);
    for (var key of formData.keys()) {
        objForm[key] = formData.get(key);
    }
    return objForm;
};

let backend = (function () {
    let todosRestfulapi = function () {
    }
    todosRestfulapi.prototype.get = function () {
        // GET request for remote todos
        return fetch('/Home/GetAll', {
            method: 'GET'
        });
    }
    todosRestfulapi.prototype.save = function (todos) {
        // Post request for remote todos: Update, Insert, Delete
        var dataSended = { todos: todos };
        return fetch('/Home/Save', {
            method: 'POST',
            body: JSON.stringify(dataSended),
            headers: {
                'Content-type': 'application/json; charset=UTF-8'
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
            .then(response => {
                if (response.ok)
                    return response.json();
                else {
                    return Promise.reject(new Error("Get User Todo List Failure!!!"));
                }
            })
            .then(result => {
                app.todos = result
                    ?.map(todo => new Todo(todo)) // 用来阻止 null引用异常. 若为 NULL 或 undefine则返回undefine, 否则继续后面的.运算操作
                    ?? [];// ?? 若为 NULL 或 undefine 则默认返回 后面的表达式值
            }, error => alert(error.message));
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
                backend.save(todos)
                    .then((response) => response.text())
                    .then((result) => console.log(result));
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
                todo_identity: (_.max(this.todos.map(t => t.identity)) || 0) + 1 ,// 取最大identity+1为新的unique identity, 没有todo时为1
                todo_textContent: value,
                todo_isCompleted: 0
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

