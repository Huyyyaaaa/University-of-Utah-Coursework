@extends('spark::layouts.login')

@section('content')
<div class="container">
    <!-- Terms of Service -->
    <div class="row justify-content-center">
        <div class="col-lg-8">
            <div class="card card-default" style="margin-top:10px;">
                <div class="card-header">{{__('Terms Of Service')}}</div>

                <div class="card-body terms-of-service">
                    {!! $terms !!}
                </div>
            </div>
        </div>
    </div>
</div>
@endsection
