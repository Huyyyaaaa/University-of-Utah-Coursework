@extends('spark::layouts.app')

@section('content')
    <project-template-show :template="{{ json_encode($project_template) }}"></project-template-show>
@endsection
