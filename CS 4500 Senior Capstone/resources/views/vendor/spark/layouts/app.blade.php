<!DOCTYPE html>
<html lang="en">
<head>
    <!-- Meta Information -->
    <meta charset="utf-8">
    <meta name="csrf-token" content="{{ csrf_token() }}">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>@yield('title', config('app.name'))</title>

    <!-- Fonts -->
    <link href='https://fonts.googleapis.com/css?family=Open+Sans:300,400,600' rel='stylesheet' type='text/css'>
    <link href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css' rel='stylesheet' type='text/css'>
    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,500,600,700&display=swap" rel="stylesheet">
    <!-- CSS -->
    <link href="{{asset('css/app.css')}}" rel="stylesheet">

    <!-- Scripts -->
    @stack('scripts')

    <!-- Global Spark Object -->
    <script>
        window.Spark = <?php echo json_encode(array_merge(Spark::scriptVariables(), [])); ?>;
    </script>
</head>
<body id="main-body">
    <div class="sidebar">
        <div class="sidebar-link-holder">
            <div class="sidebar-logo">
                <a id="sidebar-logo-link" href="#">
                    <img src="/img/logo/Regular/logo.png" alt="logo">
                </a>
                <div class="hamburger-box"><div class="hamburger"></div></div>
            </div>
           <hr class="sidebar-divider"/>
            <ul class="sidebar-links">
                <li class="sidebar-link" data-toggle="tooltip" data-placement="right" title="Dashboard">
                    <a href="/">
                        <i id="dashboard-icon" class="fas fa-chart-line"></i>
                        <span class="sidebar-text">Dashboard</span>
                    </a>
                </li>
                <li class="sidebar-link" data-toggle="tooltip" data-placement="right" title="Materials" side-step="1">
                    <a href="/material">
                        <i id="material-icon" class="fas fa-hammer"></i>
                        <span class="sidebar-text">Materials</span>
                    </a>
                </li>
                <li class="sidebar-link" data-toggle="tooltip" data-placement="right" title="Templates" side-step="2">
                 <a href="/project-template">
                    <i id="template-icon" class="fas fa-file-alt"></i>
                    <span class="sidebar-text">Templates</span>
                </a>
                </li>
                <li class="sidebar-link" data-toggle="tooltip" data-placement="right" title="Projects" side-step="3">
                    <a href="/project">
                        <i id="project-icon" class="fas fa-project-diagram"></i>
                        <span class="sidebar-text">Projects</span>
                    </a>
                </li>
                <li class="sidebar-link" data-toggle="tooltip" data-placement="right" title="Tasks" side-step="4">
                    <a href="/task">
                        <i id="task-icon" class="fas fa-tasks"></i>
                        <span class="sidebar-text">Tasks</span>
                    </a>
                </li>
                <li class="sidebar-link" data-toggle="tooltip" data-placement="right" title="Help">
                    <a href="/help">
                        <i id="help-icon" class="fas fa-info-circle"></i>
                        <span class="sidebar-text">Help</span>
                    </a>
                </li>
            </ul>
        </div>
    </div>
    <div  id="spark-app" v-cloak>
        <vue-toastr ref="mytoast"></vue-toastr>
        <!-- Navigation -->
        @if (Auth::check())
            @include('spark::nav.user')
        @else
            @include('spark::nav.guest')
        @endif

        <!-- Main Content -->
        <main>
            <div class="main-content">
                @yield('content')
            </div>
            <task-modal></task-modal>
        </main>

        <!-- Application Level Modals -->
        @if (Auth::check())
            @include('spark::modals.notifications')
            @include('spark::modals.support')
            @include('spark::modals.session-expired')
        @endif
    </div>

    <!-- JavaScript -->
    <script src="{{ mix('js/app.js') }}"></script>
    <script src="/js/sweetalert.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/gsap/3.0.5/gsap.min.js"></script>
</body>
</html>
