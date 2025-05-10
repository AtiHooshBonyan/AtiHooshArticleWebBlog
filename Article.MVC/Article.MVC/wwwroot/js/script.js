const articleListEl = document.getElementById('article-list');
const loadingEl = document.getElementById('loading');
const errorEl = document.getElementById('error');
const detailView = document.getElementById('article-detail');
const detailTitle = document.getElementById('detail-title');
const detailContent = document.getElementById('detail-content');
const closeBtn = detailView.querySelector('.close-btn');
const modal = document.getElementById('article-form-modal');
const formTitle = document.getElementById('form-title');
const titleInput = document.getElementById('article-title');
const bodyInput = document.getElementById('article-body');
const saveBtn = document.getElementById('save-article-btn');
const cancelBtn = document.getElementById('cancel-btn');
const addBtn = document.getElementById('add-btn');

let editingArticle = null;

function openForm(mode, article = null) {
    modal.style.display = 'flex';
    formTitle.textContent = mode === 'add' ? 'Add Article' : 'Update Article';

    document.getElementById('article-title').value = article?.title || '';
    document.getElementById('article-content').value = article?.content || '';
    document.getElementById('article-publishedAt').value = article?.publishedAt?.slice(0, 16) || '';
    document.getElementById('article-imagePath').value = article?.imagePath || '';
    document.getElementById('article-videoUrl').value = article?.videoUrl || '';
    document.getElementById('article-updatedAt').value = article?.updatedAt?.slice(0, 16) || '';

    editingArticle = mode === 'update' ? article : null;
}


function closeForm() {
    modal.style.display = 'none';
    titleInput.value = '';
    bodyInput.value = '';
    editingArticle = null;
}

addBtn.addEventListener('click', () => openForm('add'));
cancelBtn.addEventListener('click', closeForm);

saveBtn.addEventListener('click', async () => {
    const dto = {
        title: document.getElementById('article-title').value.trim(),
        content: document.getElementById('article-content').value.trim(),
        publishedAt: document.getElementById('article-publishedAt').value,
        imagePath: document.getElementById('article-imagePath').value.trim(),
        videoUrl: document.getElementById('article-videoUrl').value.trim(),
        updatedAt: document.getElementById('article-updatedAt').value
    };

    if (!dto.title || !dto.content) {
        alert('Title and content are required.');
        return;
    }

    const method = editingArticle ? 'PUT' : 'POST';
    const url = editingArticle ? `${API_ENDPOINT}/${editingArticle.id}` : API_ENDPOINT;

    const response = await fetch(url, {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(dto)
    });

    if (response.ok) {
        closeForm();
        fetchArticles();
    } else {
        alert('Failed to save the article.');
    }
});


const API_ENDPOINT = 'http://localhost:5201/api/articles';

function createArticleListItem(article) {
    const li = document.createElement('li');
    li.tabIndex = 0;
    li.setAttribute('role', 'button');
    li.setAttribute('aria-pressed', 'false');

    const title = document.createElement('h2');
    title.textContent = article.title;

    const excerpt = document.createElement('p');

    const controls = document.createElement('div');
    controls.className = 'article-controls';

    const editBtn = document.createElement('button');
    editBtn.textContent = 'Edit';
    editBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        openForm('update', article);

    });

    const deleteBtn = document.createElement('button');
    deleteBtn.textContent = 'Delete';
    deleteBtn.addEventListener('click', async (e) => {
        e.stopPropagation();
        if (confirm('Delete this article?')) {
            await fetch(`${API_ENDPOINT}/${article.id}`, { method: 'DELETE' });
            fetchArticles();
        }
    });

    controls.appendChild(editBtn);
    controls.appendChild(deleteBtn);

    li.appendChild(title);
    li.appendChild(excerpt);
    li.appendChild(controls);

    li.addEventListener('click', () => showArticleDetail(article));
    return li;
}


function showArticleDetail(article) {
    detailTitle.textContent = article.title;
    detailContent.textContent = article.body;
    detailView.classList.add('show');
    detailView.setAttribute('aria-hidden', 'false');
}

function closeArticleDetail() {
    detailView.classList.remove('show');
    detailView.setAttribute('aria-hidden', 'true');
}

closeBtn.addEventListener('click', closeArticleDetail);

async function fetchArticles() {
    try {
        loadingEl.style.display = 'block';
        errorEl.style.display = 'none';
        articleListEl.style.display = 'none';

        const response = await fetch(API_ENDPOINT);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const articles = await response.json();

        if (!Array.isArray(articles) || articles.length === 0) {
            loadingEl.textContent = 'No articles found.';
            return;
        }


        articleListEl.innerHTML = '';
        articles.forEach(article => {
            articleListEl.appendChild(createArticleListItem(article));
        });

        loadingEl.style.display = 'none';
        articleListEl.style.display = 'block';
    } catch (err) {
        loadingEl.style.display = 'none';
        errorEl.style.display = 'block';
        errorEl.textContent = 'Error loading articles: ' + err.message;
    }
}

fetchArticles();