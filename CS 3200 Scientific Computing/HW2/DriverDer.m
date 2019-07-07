clear;
clc;
%% write a function
syms x;
f = x.^2; %sample function
f = matlabFunction(f);

%% make a number of points for evaluating
start_point = 1;
end_point = 10;
number_of_points = 20;
points = linspace(start_point, end_point, number_of_points);
vals = f(points);
%% find the first derivative at the points
dx = FirstDer(points, vals)';
dxdf = diff(f,x);
dxdf = matlabFunction(dxdf);
derivs = dxdf(points);
%% plot 
error = zeros(length(dx), 1)';
for i=1:length(dx)
    error(i) = (abs(derivs(i) - dx(i)))/dx(i);
end

loglog(error);

